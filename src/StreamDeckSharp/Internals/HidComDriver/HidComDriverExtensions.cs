using OpenMacroBoard.SDK;

namespace StreamDeckSharp.Internals.HidComDriver;

/// <summary>
/// Some extension methods for <see cref="IStreamDeckHidComDriver"/>.
/// </summary>
public static class HidComDriverExtensions
{
    /// <summary>
    /// Generate the payload for a given <paramref name="keyBitmap"/>.
    /// </summary>
    public static byte[] GeneratePayload(this IStreamDeckHidComDriver driver, KeyBitmap keyBitmap)
    {
        var dataAccess = (IKeyBitmapDataAccess)keyBitmap;
        using var image = dataAccess.ToImage();
        return driver.GeneratePayload(image);
    }
}
