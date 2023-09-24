namespace Core.Crises;

public sealed class Crisis : ICrisis
{
    public Crisis(string sid, string eventSid)
    {
        Sid = new CrisisSid(sid);
        EventSid = eventSid;
    }

    /// <summary>
    /// Symbolic identifier of event.
    /// </summary>
    public CrisisSid Sid { get; }

    /// <summary>
    /// Small event type.
    /// </summary>
    public EventType EventType => EventType.Crisis;

    public string EventSid { get; }
}