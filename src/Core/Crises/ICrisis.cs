namespace Core.Crises;

public interface ICrisis
{
    /// <summary>
    /// Small event type.
    /// </summary>
    public EventType EventType { get; }

    /// <summary>
    /// Symbolic identifier of event.
    /// </summary>
    public CrisisSid Sid { get; }
    public string EventSid { get; }
}