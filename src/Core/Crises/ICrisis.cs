namespace Core.Crises;

public interface ICrisis
{
    public string EventSid { get; }

    /// <summary>
    /// Small event type.
    /// </summary>
    public EventType EventType { get; }

    /// <summary>
    /// Symbolic identifier of event.
    /// </summary>
    public CrisisSid Sid { get; }
}