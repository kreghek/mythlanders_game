namespace Core.Crises;

public sealed class Treasues : ICrisis
{
    public Treasues(string sid)
    {
        Sid = new CrisisSid(sid);
    }

    /// <inheritdoc/>
    public CrisisSid Sid { get; }

    /// <inheritdoc/>
    public EventType EventType => EventType.Treasues;

    /// <inheritdoc/>
    public string EventSid => $"{Sid.Value}Event";
}