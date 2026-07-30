using System;
using System.Diagnostics.CodeAnalysis;

namespace StreamDeckSharp.Exceptions
{
    /// <summary>
    /// Is thrown when the Stream Deck device cannot be accessed.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class StreamDeckAccessException
        : StreamDeckException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckAccessException"/> class.
        /// </summary>
        internal StreamDeckAccessException()
            : base("Stream Deck device is not accessible.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckAccessException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal StreamDeckAccessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDeckAccessException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.
        /// </param>
        internal StreamDeckAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
