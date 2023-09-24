namespace Core.Crises;

public interface ICrisis
{
    public EventType EventType { get; }
    public CrisisSid Sid { get; }
    public string EventSid { get; }
}