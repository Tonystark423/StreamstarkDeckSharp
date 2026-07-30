using OpenMacroBoard.SDK;
using StreamDeckSharp;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDeckSharp.Internals
{
    internal class CachedHidClient : BasicHidClient, IStreamDeck
    {
        private readonly Task writerTask;
        private readonly ConcurrentBufferedQueue<int, byte[]> imageQueue;
        private readonly ConditionalWeakTable<KeyBitmap, byte[]> cacheKeyBitmaps = new();

        public CachedHidClient(
            IStreamDeckHid deckHid,
            IKeyLayout keys,
            IStreamDeckHidComDriver hidComDriver
        )
            : base(deckHid, keys, hidComDriver)
        {
            imageQueue = new ConcurrentBufferedQueue<int, byte[]>();
            writerTask = StartBitmapWriterTask();
        }

        public override void SetKeyBitmap(int keyId, KeyBitmap bitmapData)
        {
            ThrowIfAlreadyDisposed();
            keyId = HidComDriver.ExtKeyIdToHardwareKeyId(keyId);

            var payload = cacheKeyBitmaps.GetValue(bitmapData, HidComDriver.GeneratePayload);
            imageQueue.Add(keyId, payload);
        }

        /// <summary>
        /// Asynchronously disposes the cached HID client, waiting for pending operations to complete.
        /// </summary>
        /// <returns>A ValueTask representing the asynchronous disposal operation.</returns>
        protected override async ValueTask DisposeAsyncCore()
        {
            // Signal the image queue to stop accepting new items
            imageQueue.CompleteAdding();

            // Wait for the writer task to complete asynchronously
            // This ensures all pending bitmap writes are flushed before disposal
            try
            {
                await writerTask.ConfigureAwait(false);
            }
            catch
            {
                // Suppress exceptions during disposal
            }

            // Dispose the image queue
            imageQueue.Dispose();

            // Call base async disposal
            await base.DisposeAsyncCore().ConfigureAwait(false);
        }

        protected override void Shutdown()
        {
            imageQueue.CompleteAdding();

            // Wait for the writer task to complete synchronously
            // This is called from the synchronous Dispose path
            try
            {
                writerTask?.Wait();
            }
            catch
            {
                // Suppress exceptions during disposal
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            base.Dispose(disposing);

            if (disposing)
            {
                imageQueue.Dispose();
            }
        }

        private Task StartBitmapWriterTask()
        {
            void BackgroundAction()
            {
                while (true)
                {
                    var (success, keyId, payload) = imageQueue.Take();

                    if (!success)
                    {
                        // image queue completed
                        break;
                    }

                    var reports = OutputReportSplitter.Split(
                        payload,
                        Buffer,
                        HidComDriver.ReportSize,
                        HidComDriver.HeaderSize,
                        keyId,
                        HidComDriver.PrepareDataForTransmission
                    );

                    foreach (var report in reports)
                    {
                        DeckHid.WriteReport(report);
                    }
                }
            }

            return Task.Factory.StartNew(
                BackgroundAction,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );
        }
    }
}
