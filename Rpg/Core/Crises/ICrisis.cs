namespace Core.Crises;

public interface ICrisis
{
    CrisisSid Sid { get; }

    IReadOnlyCollection<ICrisisAftermath> GetItems();

    EventType EventType { get; }
}