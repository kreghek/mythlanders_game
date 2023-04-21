namespace Client.Core;

public sealed record LocationSid(string Key) : ILocationSid
{
    public override string ToString()
    {
        return Key;
    }
}
