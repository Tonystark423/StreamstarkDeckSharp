using OpenMacroBoard.SDK;
using System;
using System.Threading.Tasks;

namespace StreamDeckSharp
{
    /// <summary>
    /// Represents a Stream Deck device with both synchronous and asynchronous disposal support.
    /// </summary>
    public interface IStreamDeck : IMacroBoard, IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the Stream Deck has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets a value indicating whether the Stream Deck is currently connected.
        /// </summary>
        bool IsConnected { get; }
    }
}
