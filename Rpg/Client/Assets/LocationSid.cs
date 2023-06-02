using Client.Core;

namespace Client.Assets;

public sealed record LocationSid(string Key) : ILocationSid
{
    public override string ToString()
    {
        return Key;
    }
}