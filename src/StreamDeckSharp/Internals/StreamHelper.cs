using System;
using System.IO;

namespace StreamDeckSharp.Internals;

internal static class StreamHelper
{
    public static byte[] WriteArray(Action<Stream> streamer)
    {
        using var ms = new MemoryStream();
        streamer(ms);
        return ms.ToArray();
    }
}
