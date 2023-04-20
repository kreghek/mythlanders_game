namespace Core.Crises;

public interface ICrisis
{
    EventType EventType { get; }
    CrisisSid Sid { get; }

    IReadOnlyCollection<ICrisisAftermath> GetItems();
}