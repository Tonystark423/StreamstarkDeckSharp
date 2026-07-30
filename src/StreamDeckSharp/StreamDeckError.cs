namespace StreamDeckSharp
{
    /// <summary>
    /// Defines error codes for Stream Deck operations.
    /// </summary>
    public enum StreamDeckError
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Stream Deck device was not found.
        /// </summary>
        DeviceNotFound = 1,

        /// <summary>
        /// The Stream Deck device cannot be accessed (permissions, exclusive access, etc.).
        /// </summary>
        DeviceNotAccessible = 2,

        /// <summary>
        /// The Stream Deck device has invalid or incompatible firmware.
        /// </summary>
        InvalidFirmware = 3,

        /// <summary>
        /// A USB transfer operation failed.
        /// </summary>
        UsbTransferFailed = 4,

        /// <summary>
        /// The connection to the Stream Deck device was lost.
        /// </summary>
        ConnectionLost = 5,

        /// <summary>
        /// The connection to the Stream Deck device could not be established.
        /// </summary>
        ConnectionFailed = 6,

        /// <summary>
        /// The Stream Deck device is already in use by another application.
        /// </summary>
        DeviceInUse = 7,

        /// <summary>
        /// The operation timed out.
        /// </summary>
        Timeout = 8,

        /// <summary>
        /// The device returned an invalid response.
        /// </summary>
        InvalidResponse = 9,

        /// <summary>
        /// The requested operation is not supported by this device.
        /// </summary>
        OperationNotSupported = 10,

        /// <summary>
        /// The device is not in the correct state for the requested operation.
        /// </summary>
        InvalidDeviceState = 11,

        /// <summary>
        /// A memory allocation failure occurred.
        /// </summary>
        OutOfMemory = 12,

        /// <summary>
        /// An I/O error occurred during the operation.
        /// </summary>
        IoError = 13,

        /// <summary>
        /// The device was disconnected during the operation.
        /// </summary>
        DeviceDisconnected = 14
    }
}
