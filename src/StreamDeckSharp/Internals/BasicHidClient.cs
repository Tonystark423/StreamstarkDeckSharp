using OpenMacroBoard.SDK;
using StreamDeckSharp;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDeckSharp.Internals
{
    internal class BasicHidClient : IStreamDeck
    {
        private readonly byte[] keyStates;
        private readonly object disposeLock = new();
        private bool _disposed;

        public BasicHidClient(
            IStreamDeckHid deckHid,
            IKeyLayout keys,
            IStreamDeckHidComDriver hidComDriver
        )
        {
            DeckHid = deckHid;
            Keys = keys;

            deckHid.ConnectionStateChanged += (_, e) => ConnectionStateChanged?.Invoke(this, e);
            deckHid.ReportReceived += DeckHid_ReportReceived;

            HidComDriver = hidComDriver;
            Buffer = new byte[deckHid.OutputReportLength];
            keyStates = new byte[Keys.Count];
        }

        public event EventHandler<KeyEventArgs> KeyStateChanged;
        public event EventHandler<ConnectionEventArgs> ConnectionStateChanged;

        public IKeyLayout Keys { get; }
        public bool IsDisposed => _disposed;
        public bool IsConnected => DeckHid.IsConnected;

        protected IStreamDeckHid DeckHid { get; }
        protected IStreamDeckHidComDriver HidComDriver { get; }
        protected byte[] Buffer { get; }

        /// <summary>
        /// Disposes the Stream Deck client and releases all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Asynchronously disposes the Stream Deck client and releases all resources.
        /// </summary>
        /// <returns>A ValueTask representing the asynchronous disposal operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public string GetFirmwareVersion()
        {
            ThrowIfAlreadyDisposed();
            return ReadFeatureString(HidComDriver.FirmwareVersionFeatureId, HidComDriver.FirmwareVersionReportSkip);
        }

        public string GetSerialNumber()
        {
            ThrowIfAlreadyDisposed();
            return ReadFeatureString(HidComDriver.SerialNumberFeatureId, HidComDriver.SerialNumberReportSkip);
        }

        public void SetBrightness(byte percent)
        {
            ThrowIfAlreadyDisposed();
            DeckHid.WriteFeature(HidComDriver.GetBrightnessMessage(percent));
        }

        public virtual void SetKeyBitmap(int keyId, KeyBitmap bitmapData)
        {
            ThrowIfAlreadyDisposed();
            keyId = HidComDriver.ExtKeyIdToHardwareKeyId(keyId);

            var payload = HidComDriver.GeneratePayload(bitmapData);

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

        public void ShowLogo()
        {
            ThrowIfAlreadyDisposed();
            ShowLogoWithoutDisposeVerification();
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        protected bool Disposed => _disposed;

        protected virtual void Shutdown()
        {
        }

        /// <summary>
        /// Asynchronously performs cleanup operations.
        /// </summary>
        /// <returns>A ValueTask representing the asynchronous cleanup.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            // Default implementation does nothing asynchronously
            // Derived classes can override to add async cleanup
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Performs cleanup operations.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                lock (disposeLock)
                {
                    if (_disposed)
                    {
                        return;
                    }

                    _disposed = true;
                }

                Shutdown();

                // Sleep to let the stream deck catch up.
                // Without this Sleep() the stream deck might set a key image after the logo was shown.
                // I've no idea why it's sometimes executed out of order even though the write is synchronized.
                Thread.Sleep(50);

                ShowLogoWithoutDisposeVerification();

                DeckHid.Dispose();
            }
        }

        protected void ThrowIfAlreadyDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(BasicHidClient));
            }
        }

        private string ReadFeatureString(byte featureId, int skipBytes)
        {
            if (!DeckHid.ReadFeatureData(featureId, out var featureData))
            {
#pragma warning disable AV1135 // Do not return null for strings, collections or tasks
                return null;
#pragma warning restore AV1135
            }

            return Encoding.UTF8.GetString(featureData, skipBytes, featureData.Length - skipBytes).Trim('\0');
        }

        private void DeckHid_ReportReceived(object sender, ReportReceivedEventArgs e)
        {
            ProcessKeys(e.ReportData);
        }

        private void ProcessKeys(byte[] newStates)
        {
            for (var i = 0; i < keyStates.Length; i++)
            {
                var newStatePos = i + HidComDriver.KeyReportOffset;

                if (keyStates[i] != newStates[newStatePos])
                {
                    var externalKeyId = HidComDriver.HardwareKeyIdToExtKeyId(i);
                    KeyStateChanged?.Invoke(this, new KeyEventArgs(externalKeyId, newStates[newStatePos] != 0));
                    keyStates[i] = newStates[newStatePos];
                }
            }
        }

        private void ShowLogoWithoutDisposeVerification()
        {
            DeckHid.WriteFeature(HidComDriver.GetLogoMessage());
        }
    }
}
