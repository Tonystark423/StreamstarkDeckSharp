using System;
using System.Diagnostics.CodeAnalysis;

namespace StreamDeckSharp.Exceptions
{
    /// <summary>
    /// Is thrown when a USB transfer operation fails.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class StreamDeckUsbTransferException
        : StreamDeckException
    {
        /// <summary>
        /// Gets the operation type that failed.
        /// </summary>
        public UsbOperationType OperationType { get; }

        /// <summary>
        /// Gets the endpoint address involved in the failed operation.
        /// </summary>
        public int? EndpointAddress { get; }

        /// <summary>
        /// Gets the number of bytes attempted to transfer.
        /// </summary>
        public int BytesTransferred { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckUsbTransferException"/> class.
        /// </summary>
        internal StreamDeckUsbTransferException()
            : base("USB transfer operation failed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckUsbTransferException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal StreamDeckUsbTransferException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckUsbTransferException"/> class.
        /// </summary>
        /// <param name="operationType">The type of USB operation that failed.</param>
        /// <param name="bytesTransferred">The number of bytes attempted to transfer.</param>
        internal StreamDeckUsbTransferException(UsbOperationType operationType, int bytesTransferred)
            : base($