# StreamDeckSharp Disposal Pattern and Error Handling Improvements

## Overview

This document describes the improvements made to the StreamDeckSharp library to implement proper disposal patterns and comprehensive error handling.

## Changes Made

### 1. New Exception Classes

Added the following exception classes to the `StreamDeckSharp.Exceptions` namespace:

- **`StreamDeckAccessException`**: Thrown when the Stream Deck device cannot be accessed (permissions, exclusive access, etc.)
- **`StreamDeckFirmwareException`**: Thrown when the Stream Deck device has invalid or incompatible firmware
- **`StreamDeckUsbTransferException`**: Thrown when a USB transfer operation fails, with detailed information about the operation type, endpoint, and bytes transferred
- **`StreamDeckConnectionException`**: Thrown when there is a connection problem with the Stream Deck device

### 2. StreamDeckError Enum

Created `StreamDeckError.cs` with a comprehensive enum defining all possible error codes:

```csharp
public enum StreamDeckError
{
    None = 0,
    DeviceNotFound = 1,
    DeviceNotAccessible = 2,
    InvalidFirmware = 3,
    UsbTransferFailed = 4,
    ConnectionLost = 5,
    ConnectionFailed = 6,
    DeviceInUse = 7,
    Timeout = 8,
    InvalidResponse = 9,
    OperationNotSupported = 10,
    InvalidDeviceState = 11,
    OutOfMemory = 12,
    IoError = 13,
    DeviceDisconnected = 14
}
```

### 3. IStreamDeck Interface

Created `IStreamDeck.cs` interface that extends `IMacroBoard` with both `IDisposable` and `IAsyncDisposable`:

```csharp
public interface IStreamDeck : IMacroBoard, IDisposable, IAsyncDisposable
{
    bool IsDisposed { get; }
    bool IsConnected { get; }
}
```

### 4. BasicHidClient Improvements

Updated `BasicHidClient.cs` to:

- Implement `IStreamDeck` interface
- Add `IAsyncDisposable` support with `DisposeAsync()` method
- Improve disposal pattern with proper locking and state management
- Add `ThrowIfAlreadyDisposed()` method for consistent disposal checking
- Add `Disposed` protected property for derived classes
- Add `DisposeAsyncCore()` virtual method for async cleanup

### 5. CachedHidClient Improvements

Updated `CachedHidClient.cs` to:

- Implement `IStreamDeck` interface
- Override `DisposeAsyncCore()` to properly await the writer task completion
- Improve `Shutdown()` method to handle task completion
- Update `Dispose()` to properly clean up resources

### 6. IStreamDeckHid Interface

Updated `IStreamDeckHid.cs` to extend `IAsyncDisposable`:

```csharp
internal interface IStreamDeckHid : IDisposable, IAsyncDisposable
{
    // ... existing members
}
```

### 7. StreamDeckHidWrapper Improvements

Updated `StreamDeckHidWrapper.cs` to:

- Add `IsDisposed` property
- Implement `IAsyncDisposable` with `DisposeAsync()` method
- Add `DisposeAsyncCore()` method for async cleanup
- Improve `Dispose()` method with proper state management
- Add proper cleanup of event handlers and resources

## Usage Examples

### Synchronous Disposal

```csharp
using (var deck = StreamDeck.OpenDevice())
{
    deck.SetBrightness(50);
    // ... use the deck
} // Automatically calls Dispose()
```

### Asynchronous Disposal

```csharp
await using (var deck = StreamDeck.OpenDevice())
{
    deck.SetBrightness(50);
    // ... use the deck
} // Automatically calls DisposeAsync()
```

### Manual Disposal

```csharp
var deck = StreamDeck.OpenDevice();
try
{
    deck.SetBrightness(50);
    // ... use the deck
}
finally
{
    // Synchronous disposal
    deck.Dispose();
    
    // Or asynchronous disposal
    await deck.DisposeAsync();
}
```

### Error Handling

```csharp
try
{
    var deck = StreamDeck.OpenDevice();
    // ... use the deck
}
catch (StreamDeckNotFoundException ex)
{
    // Handle device not found
    Console.WriteLine("Stream Deck not found: " + ex.Message);
}
catch (StreamDeckAccessException ex)
{
    // Handle access denied
    Console.WriteLine("Access denied: " + ex.Message);
}
catch (StreamDeckFirmwareException ex)
{
    // Handle firmware incompatibility
    Console.WriteLine($