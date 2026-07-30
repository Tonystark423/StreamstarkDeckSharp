using StreamDeckSharp.Exceptions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace StreamDeckSharp.Tests
{
    /// <summary>
    /// Tests for the new exception classes.
    /// </summary>
    public class ExceptionTests
    {
        [Fact]
        public void StreamDeckException_ShouldBeAbstract()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAbstract);
        }

        [Fact]
        public void StreamDeckException_ShouldInheritFromException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(Exception).IsAssignableFrom(typeof(StreamDeckException)));
        }

        [Fact]
        public void StreamDeckNotFoundException_ShouldInheritFromStreamDeckException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAssignableFrom(typeof(StreamDeckNotFoundException)));
        }

        [Fact]
        public void StreamDeckAccessException_ShouldInheritFromStreamDeckException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAssignableFrom(typeof(StreamDeckAccessException)));
        }

        [Fact]
        public void StreamDeckConnectionException_ShouldInheritFromStreamDeckException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAssignableFrom(typeof(StreamDeckConnectionException)));
        }

        [Fact]
        public void StreamDeckFirmwareException_ShouldInheritFromStreamDeckException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAssignableFrom(typeof(StreamDeckFirmwareException)));
        }

        [Fact]
        public void StreamDeckUsbTransferException_ShouldInheritFromStreamDeckException()
        {
            // Arrange & Act & Assert
            Assert.True(typeof(StreamDeckException).IsAssignableFrom(typeof(StreamDeckUsbTransferException)));
        }
    }

    /// <summary>
    /// Tests for StreamDeckAccessException.
    /// </summary>
    public class StreamDeckAccessExceptionTests
    {
        [Fact]
        public void DefaultConstructor_ShouldSetDefaultMessage()
        {
            // Arrange
            var exception = new StreamDeckAccessException();

            // Act & Assert
            Assert.Equal("Stream Deck device is not accessible.", exception.Message);
        }

        [Fact]
        public void MessageConstructor_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Custom access denied message";

            // Act
            var exception = new StreamDeckAccessException(customMessage);

            // Assert
            Assert.Equal(customMessage, exception.Message);
        }

        [Fact]
        public void MessageAndInnerExceptionConstructor_ShouldSetBoth()
        {
            // Arrange
            var customMessage = "Custom access denied message";
            var innerException = new IOException("Test IO error");

            // Act
            var exception = new StreamDeckAccessException(customMessage, innerException);

            // Assert
            Assert.Equal(customMessage, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void ShouldBeSerializable()
        {
            // Arrange
            var exception = new StreamDeckAccessException("Test message");
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            // Act
            formatter.Serialize(stream, exception);
            stream.Position = 0;
            var deserialized = (StreamDeckAccessException)formatter.Deserialize(stream);

            // Assert
            Assert.Equal(exception.Message, deserialized.Message);
        }
    }

    /// <summary>
    /// Tests for StreamDeckConnectionException.
    /// </summary>
    public class StreamDeckConnectionExceptionTests
    {
        [Fact]
        public void DefaultConstructor_ShouldSetDefaultMessage()
        {
            // Arrange
            var exception = new StreamDeckConnectionException();

            // Act & Assert
            Assert.Equal("Connection to Stream Deck device was lost.", exception.Message);
        }

        [Fact]
        public void MessageConstructor_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Custom connection error message";

            // Act
            var exception = new StreamDeckConnectionException(customMessage);

            // Assert
            Assert.Equal(customMessage, exception.Message);
        }

        [Fact]
        public void DevicePathAndWasConnectedConstructor_ShouldSetProperties()
        {
            // Arrange
            var devicePath = "/dev/hidraw0";
            var wasConnected = true;

            // Act
            var exception = new StreamDeckConnectionException(devicePath, wasConnected);

            // Assert
            Assert.Equal(devicePath, exception.DevicePath);
            Assert.Equal(wasConnected, exception.WasConnected);
            Assert.Contains(devicePath, exception.Message);
        }

        [Fact]
        public void DevicePathAndWasConnectedConstructor_WhenNotConnected_ShouldSetMessage()
        {
            // Arrange
            var devicePath = "/dev/hidraw0";
            var wasConnected = false;

            // Act
            var exception = new StreamDeckConnectionException(devicePath, wasConnected);

            // Assert
            Assert.Equal(devicePath, exception.DevicePath);
            Assert.Equal(wasConnected, exception.WasConnected);
            Assert.Contains("Cannot establish connection", exception.Message);
        }

        [Fact]
        public void FullConstructor_ShouldSetAllProperties()
        {
            // Arrange
            var devicePath = "/dev/hidraw0";
            var wasConnected = true;
            var innerException = new IOException("Test IO error");

            // Act
            var exception = new StreamDeckConnectionException(devicePath, wasConnected, innerException);

            // Assert
            Assert.Equal(devicePath, exception.DevicePath);
            Assert.Equal(wasConnected, exception.WasConnected);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void ShouldBeSerializable()
        {
            // Arrange
            var exception = new StreamDeckConnectionException("/dev/hidraw0", true);
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            // Act
            formatter.Serialize(stream, exception);
            stream.Position = 0;
            var deserialized = (StreamDeckConnectionException)formatter.Deserialize(stream);

            // Assert
            Assert.Equal(exception.Message, deserialized.Message);
            Assert.Equal(exception.DevicePath, deserialized.DevicePath);
            Assert.Equal(exception.WasConnected, deserialized.WasConnected);
        }
    }

    /// <summary>
    /// Tests for StreamDeckFirmwareException.
    /// </summary>
    public class StreamDeckFirmwareExceptionTests
    {
        [Fact]
        public void DefaultConstructor_ShouldSetDefaultMessage()
        {
            // Arrange
            var exception = new StreamDeckFirmwareException();

            // Act & Assert
            Assert.Equal("Stream Deck device has invalid or incompatible firmware.", exception.Message);
        }

        [Fact]
        public void MessageConstructor_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Custom firmware error message";

            // Act
            var exception = new StreamDeckFirmwareException(customMessage);

            // Assert
            Assert.Equal(customMessage, exception.Message);
        }

        [Fact]
        public void FirmwareVersionConstructor_ShouldSetProperties()
        {
            // Arrange
            var firmwareVersion = "1.0.0";
            var expectedVersion = "2.0.0";

            // Act
            var exception = new StreamDeckFirmwareException(firmwareVersion, expectedVersion);

            // Assert
            Assert.Equal(firmwareVersion, exception.FirmwareVersion);
            Assert.Equal(expectedVersion, exception.ExpectedVersion);
            Assert.Contains(firmwareVersion, exception.Message);
            Assert.Contains(expectedVersion, exception.Message);
        }

        [Fact]
        public void FullConstructor_ShouldSetAllProperties()
        {
            // Arrange
            var firmwareVersion = "1.0.0";
            var expectedVersion = "2.0.0";
            var innerException = new IOException("Test IO error");

            // Act
            var exception = new StreamDeckFirmwareException(firmwareVersion, expectedVersion, innerException);

            // Assert
            Assert.Equal(firmwareVersion, exception.FirmwareVersion);
            Assert.Equal(expectedVersion, exception.ExpectedVersion);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void ShouldBeSerializable()
        {
            // Arrange
            var exception = new StreamDeckFirmwareException("1.0.0", "2.0.0");
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            // Act
            formatter.Serialize(stream, exception);
            stream.Position = 0;
            var deserialized = (StreamDeckFirmwareException)formatter.Deserialize(stream);

            // Assert
            Assert.Equal(exception.Message, deserialized.Message);
            Assert.Equal(exception.FirmwareVersion, deserialized.FirmwareVersion);
            Assert.Equal(exception.ExpectedVersion, deserialized.ExpectedVersion);
        }
    }

    /// <summary>
    /// Tests for StreamDeckUsbTransferException.
    /// </summary>
    public class StreamDeckUsbTransferExceptionTests
    {
        [Fact]
        public void DefaultConstructor_ShouldSetDefaultMessage()
        {
            // Arrange
            var exception = new StreamDeckUsbTransferException();

            // Act & Assert
            Assert.Equal("USB transfer operation failed.", exception.Message);
        }

        [Fact]
        public void MessageConstructor_ShouldSetCustomMessage()
        {
            // Arrange
            var customMessage = "Custom USB transfer error message";

            // Act
            var exception = new StreamDeckUsbTransferException(customMessage);

            // Assert
            Assert.Equal(customMessage, exception.Message);
        }

        [Fact]
        public void OperationTypeAndBytesConstructor_ShouldSetProperties()
        {
            // Arrange
            var operationType = StreamDeckUsbTransferException.UsbOperationType.Write;
            var bytesTransferred = 1024;

            // Act
            var exception = new StreamDeckUsbTransferException(operationType, bytesTransferred);

            // Assert
            Assert.Equal(operationType, exception.OperationType);
            Assert.Equal(bytesTransferred, exception.BytesTransferred);
            Assert.Contains(operationType.ToString(), exception.Message);
            Assert.Contains(bytesTransferred.ToString(), exception.Message);
        }

        [Fact]
        public void FullConstructor_ShouldSetAllProperties()
        {
            // Arrange
            var operationType = StreamDeckUsbTransferException.UsbOperationType.Read;
            var endpointAddress = 0x81;
            var bytesTransferred = 512;

            // Act
            var exception = new StreamDeckUsbTransferException(operationType, endpointAddress, bytesTransferred);

            // Assert
            Assert.Equal(operationType, exception.OperationType);
            Assert.Equal(endpointAddress, exception.EndpointAddress);
            Assert.Equal(bytesTransferred, exception.BytesTransferred);
            Assert.Contains(endpointAddress.ToString("X2"), exception.Message);
        }

        [Fact]
        public void FullConstructorWithInnerException_ShouldSetAllProperties()
        {
            // Arrange
            var operationType = StreamDeckUsbTransferException.UsbOperationType.ControlTransfer;
            var bytesTransferred = 256;
            var innerException = new IOException("Test IO error");

            // Act
            var exception = new StreamDeckUsbTransferException(
                operationType, bytesTransferred, innerException);

            // Assert
            Assert.Equal(operationType, exception.OperationType);
            Assert.Equal(bytesTransferred, exception.BytesTransferred);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void EndpointAddress_WhenNotSet_ShouldBeNull()
        {
            // Arrange
            var exception = new StreamDeckUsbTransferException(
                StreamDeckUsbTransferException.UsbOperationType.Write, 1024);

            // Act & Assert
            Assert.Null(exception.EndpointAddress);
        }

        [Fact]
        public void ShouldBeSerializable()
        {
            // Arrange
            var exception = new StreamDeckUsbTransferException(
                StreamDeckUsbTransferException.UsbOperationType.Write, 0x81, 1024);
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            // Act
            formatter.Serialize(stream, exception);
            stream.Position = 0;
            var deserialized = (StreamDeckUsbTransferException)formatter.Deserialize(stream);

            // Assert
            Assert.Equal(exception.Message, deserialized.Message);
            Assert.Equal(exception.OperationType, deserialized.OperationType);
            Assert.Equal(exception.EndpointAddress, deserialized.EndpointAddress);
            Assert.Equal(exception.BytesTransferred, deserialized.BytesTransferred);
        }
    }

    /// <summary>
    /// Tests for StreamDeckError enum.
    /// </summary>
    public class StreamDeckErrorEnumTests
    {
        [Fact]
        public void None_ShouldBeZero()
        {
            // Arrange & Act & Assert
            Assert.Equal(0, (int)StreamDeckError.None);
        }

        [Fact]
        public void AllValues_ShouldBeUnique()
        {
            // Arrange
            var values = Enum.GetValues(typeof(StreamDeckError));

            // Act & Assert
            var hashSet = new System.Collections.Generic.HashSet<int>();
            foreach (var value in values)
            {
                var intValue = (int)value;
                Assert.DoesNotContain(intValue, hashSet);
                hashSet.Add(intValue);
            }
        }

        [Fact]
        public void AllValues_ShouldBeDefined()
        {
            // Arrange
            var expectedValues = new[]
            {
                StreamDeckError.None,
                StreamDeckError.DeviceNotFound,
                StreamDeckError.DeviceNotAccessible,
                StreamDeckError.InvalidFirmware,
                StreamDeckError.UsbTransferFailed,
                StreamDeckError.ConnectionLost,
                StreamDeckError.ConnectionFailed,
                StreamDeckError.DeviceInUse,
                StreamDeckError.Timeout,
                StreamDeckError.InvalidResponse,
                StreamDeckError.OperationNotSupported,
                StreamDeckError.InvalidDeviceState,
                StreamDeckError.OutOfMemory,
                StreamDeckError.IoError,
                StreamDeckError.DeviceDisconnected
            };

            // Act
            var actualValues = Enum.GetValues(typeof(StreamDeckError));

            // Assert
            Assert.Equal(expectedValues.Length, actualValues.Length);
            foreach (var expected in expectedValues)
            {
                Assert.Contains(expected, actualValues);
            }
        }

        [Fact]
        public void ToString_ShouldReturnMeaningfulNames()
        {
            // Arrange
            var error = StreamDeckError.DeviceNotFound;

            // Act
            var name = error.ToString();

            // Assert
            Assert.Equal("DeviceNotFound", name);
        }
    }
}
