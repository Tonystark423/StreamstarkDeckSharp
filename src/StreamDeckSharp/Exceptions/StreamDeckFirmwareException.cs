using System;
using System.Diagnostics.CodeAnalysis;

namespace StreamDeckSharp.Exceptions
{
    /// <summary>
    /// Is thrown when the Stream Deck device has incompatible or invalid firmware.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class StreamDeckFirmwareException
        : StreamDeckException
    {
        /// <summary>
        /// Gets the detected firmware version.
        /// </summary>
        public string FirmwareVersion { get; }

        /// <summary>
        /// Gets the expected firmware version.
        /// </summary>
        public string ExpectedVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckFirmwareException"/> class.
        /// </summary>
        internal StreamDeckFirmwareException()
            : base("Stream Deck device has invalid or incompatible firmware.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckFirmwareException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal StreamDeckFirmwareException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckFirmwareException"/> class.
        /// </summary>
        /// <param name="firmwareVersion">The detected firmware version.</param>
        /// <param name="expectedVersion">The expected firmware version.</param>
        internal StreamDeckFirmwareException(string firmwareVersion, string expectedVersion)
            : base($