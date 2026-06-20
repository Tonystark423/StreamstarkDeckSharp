namespace StreamDeckSharp.Internals.HidComDriver;

/// <summary>
/// The identity key mapper does not transformation at all
/// and returns the same ID back that was given.
/// </summary>
public class IdentityKeyMapper : IKeyIdMapper
{
    /// <inheritdoc/>
    public int ExtKeyIdToHardwareKeyId(int extKeyId)
    {
        return extKeyId;
    }

    /// <inheritdoc/>
    public int HardwareKeyIdToExtKeyId(int hardwareKeyId)
    {
        return hardwareKeyId;
    }
}
