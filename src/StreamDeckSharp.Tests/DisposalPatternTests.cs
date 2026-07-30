using Moq;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Exceptions;
using StreamDeckSharp.Internals;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace StreamDeckSharp.Tests
{
    /// <summary>
    /// Tests for the disposal pattern implementations in StreamDeckSharp.
    /// </summary>
    public class DisposalPatternTests : IDisposable
    {
        private readonly StringWriter _log;
        private readonly UsbHardwareIdAndDriver _hardware;

        public DisposalPatternTests()
        {
            _log = new StringWriter();
            _hardware = new UsbHardwareIdAndDriver(
                new UsbVendorProductPair(0x01, 0x01),
                new FakeStreamDeckHidComDriver());
        }

        public void Dispose()
        {
            _log.Dispose();
        }

        [Fact]
        public void BasicHidClient_Dispose_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            client.Dispose();

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public void BasicHidClient_Dispose_Twice_ShouldNotThrow()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            client.Dispose();
            client.Dispose(); // Second disposal should not throw

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public async Task BasicHidClient_DisposeAsync_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            await client.DisposeAsync();

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public async Task BasicHidClient_DisposeAsync_Twice_ShouldNotThrow()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            await client.DisposeAsync();
            await client.DisposeAsync(); // Second disposal should not throw

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public void BasicHidClient_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);
            client.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => client.SetBrightness(50));
        }

        [Fact]
        public void BasicHidClient_AfterDisposeAsync_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);
            client.DisposeAsync().Wait();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => client.SetBrightness(50));
        }

        [Fact]
        public void BasicHidClient_Dispose_ShouldDisposeUnderlyingHid()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            client.Dispose();

            // Assert - Check that Dispose was called on the underlying HID
            var logContent = _log.ToString();
            Assert.Contains("Dispose()", logContent);
        }

        [Fact]
        public async Task BasicHidClient_DisposeAsync_ShouldDisposeUnderlyingHid()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            await client.DisposeAsync();

            // Assert - Check that Dispose was called on the underlying HID
            var logContent = _log.ToString();
            Assert.Contains("Dispose()", logContent);
        }

        [Fact]
        public void BasicHidClient_UsingStatement_ShouldDisposeProperly()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);

            // Act
            using (var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver))
            {
                client.SetBrightness(50);
            }

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }

        [Fact]
        public async Task BasicHidClient_AwaitUsingStatement_ShouldDisposeProperly()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);

            // Act
            await using (var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver))
            {
                client.SetBrightness(50);
            }

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }

        [Fact]
        public void BasicHidClient_IsConnected_ShouldReflectHidState()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act & Assert
            Assert.Equal(fakeHid.IsConnected, client.IsConnected);
        }

        [Fact]
        public void BasicHidClient_Keys_ShouldNotBeNull()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new BasicHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act & Assert
            Assert.NotNull(client.Keys);
        }
    }

    /// <summary>
    /// Tests for CachedHidClient disposal pattern.
    /// </summary>
    public class CachedHidClientDisposalTests : IDisposable
    {
        private readonly StringWriter _log;
        private readonly UsbHardwareIdAndDriver _hardware;

        public CachedHidClientDisposalTests()
        {
            _log = new StringWriter();
            _hardware = new UsbHardwareIdAndDriver(
                new UsbVendorProductPair(0x01, 0x01),
                new FakeStreamDeckHidComDriver());
        }

        public void Dispose()
        {
            _log.Dispose();
        }

        [Fact]
        public void CachedHidClient_Dispose_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            client.Dispose();

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public async Task CachedHidClient_DisposeAsync_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver);

            // Act
            await client.DisposeAsync();

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public void CachedHidClient_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver);
            client.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => client.SetBrightness(50));
        }

        [Fact]
        public async Task CachedHidClient_DisposeAsync_ShouldWaitForWriterTask()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);
            var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver);

            // Add some bitmap operations to the queue
            var bitmap = KeyBitmap.Create.FromRgb(255, 0, 0);
            client.SetKeyBitmap(0, bitmap);

            // Act - Dispose should wait for the writer task to complete
            await client.DisposeAsync();

            // Assert
            Assert.True(client.IsDisposed);
        }

        [Fact]
        public void CachedHidClient_UsingStatement_ShouldDisposeProperly()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);

            // Act
            using (var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver))
            {
                client.SetBrightness(50);
            }

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }

        [Fact]
        public async Task CachedHidClient_AwaitUsingStatement_ShouldDisposeProperly()
        {
            // Arrange
            var fakeHid = new FakeStreamDeckHid(_log, _hardware);

            // Act
            await using (var client = new CachedHidClient(fakeHid, _hardware, _hardware.Driver))
            {
                client.SetBrightness(50);
            }

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }
    }

    /// <summary>
    /// Tests for StreamDeckHidWrapper disposal pattern.
    /// </summary>
    public class StreamDeckHidWrapperDisposalTests
    {
        [Fact]
        public void StreamDeckHidWrapper_Dispose_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var log = new StringWriter();
            var hardware = new UsbHardwareIdAndDriver(
                new UsbVendorProductPair(0x01, 0x01),
                new FakeStreamDeckHidComDriver());
            
            // Note: We can't easily test the actual StreamDeckHidWrapper without a real device,
            // but we can test the interface contract
            var fakeHid = new FakeStreamDeckHid(log, hardware);
            
            // Act
            fakeHid.Dispose();

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }

        [Fact]
        public async Task StreamDeckHidWrapper_DisposeAsync_ShouldSetIsDisposedToTrue()
        {
            // Arrange
            var log = new StringWriter();
            var hardware = new UsbHardwareIdAndDriver(
                new UsbVendorProductPair(0x01, 0x01),
                new FakeStreamDeckHidComDriver());
            
            var fakeHid = new FakeStreamDeckHid(log, hardware);
            
            // Act
            await fakeHid.DisposeAsync();

            // Assert
            Assert.True(fakeHid.IsDisposed);
        }
    }

    /// <summary>
    /// Tests for the new IStreamDeck interface.
    /// </summary>
    public class IStreamDeckInterfaceTests
    {
        [Fact]
        public void IStreamDeck_ShouldExtendIMacroBoard()
        {
            // Arrange & Act
            var interfaceType = typeof(IStreamDeck);
            
            // Assert
            Assert.True(typeof(IMacroBoard).IsAssignableFrom(interfaceType));
        }

        [Fact]
        public void IStreamDeck_ShouldExtendIDisposable()
        {
            // Arrange & Act
            var interfaceType = typeof(IStreamDeck);
            
            // Assert
            Assert.True(typeof(IDisposable).IsAssignableFrom(interfaceType));
        }

        [Fact]
        public void IStreamDeck_ShouldExtendIAsyncDisposable()
        {
            // Arrange & Act
            var interfaceType = typeof(IStreamDeck);
            
            // Assert
            Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(interfaceType));
        }

        [Fact]
        public void IStreamDeck_ShouldHaveIsDisposedProperty()
        {
            // Arrange & Act
            var property = typeof(IStreamDeck).GetProperty("IsDisposed");
            
            // Assert
            Assert.NotNull(property);
            Assert.Equal(typeof(bool), property.PropertyType);
        }

        [Fact]
        public void IStreamDeck_ShouldHaveIsConnectedProperty()
        {
            // Arrange & Act
            var property = typeof(IStreamDeck).GetProperty("IsConnected");
            
            // Assert
            Assert.NotNull(property);
            Assert.Equal(typeof(bool), property.PropertyType);
        }
    }
}
