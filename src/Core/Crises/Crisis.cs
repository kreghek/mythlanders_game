namespace Core.Crises;

public sealed class Crisis : ICrisis
{
    public Crisis(string sid)
    {
        Sid = new CrisisSid(sid);
    }

    /// <summary>
    /// Symbolic identifier of event.
    /// </summary>
    public CrisisSid Sid { get; }

    /// <summary>
    /// Small event type.
    /// </summary>
    public EventType EventType => EventType.Crisis;

    public string EventSid => $"{Sid.Value}Event";
}