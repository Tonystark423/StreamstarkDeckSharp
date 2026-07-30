using System;
using System.Diagnostics.CodeAnalysis;

namespace StreamDeckSharp.Exceptions
{
    /// <summary>
    /// Is thrown when there is a connection problem with the Stream Deck device.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class StreamDeckConnectionException
        : StreamDeckException
    {
        /// <summary>
        /// Gets the connection state at the time of the error.
        /// </summary>
        public bool WasConnected { get; }

        /// <summary>
        /// Gets the device path of the affected device.
        /// </summary>
        public string DevicePath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckConnectionException"/> class.
        /// </summary>
        internal StreamDeckConnectionException()
            : base("Connection to Stream Deck device was lost.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckConnectionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal StreamDeckConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckConnectionException"/> class.
        /// </summary>
        /// <param name="devicePath">The device path of the affected device.</param>
        /// <param name="wasConnected">Whether the device was connected at the time of the error.</param>
        internal StreamDeckConnectionException(string devicePath, bool wasConnected)
            : base(wasConnected
                ? $"    ,  	Connection to Stream Deck at '{devicePath}' was lost."
                : $"    ,  	Cannot establish connection to Stream Deck at '{devicePath}'.")
        {
            DevicePath = devicePath;
            WasConnected = wasConnected;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckConnectionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.
        /// </param>
        internal StreamDeckConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckConnectionException"/> class.
        /// </summary>
        /// <param name="devicePath">The device path of the affected device.</param>
        /// <param name="wasConnected">Whether the device was connected at the time of the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.
        /// </param>
        internal StreamDeckConnectionException(string devicePath, bool wasConnected, Exception innerException)
            : base(wasConnected
                ? $"    ,  	Connection to Stream Deck at '{devicePath}' was lost."
                : $"    ,  	Cannot establish connection to Stream Deck at '{devicePath}'.", innerException)
        {
            DevicePath = devicePath;
            WasConnected = wasConnected;
        }
    }
}
