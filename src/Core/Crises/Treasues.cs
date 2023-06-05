namespace Core.Crises;

public sealed class Treasues : ICrisis
{
    private readonly IReadOnlyCollection<ICrisisAftermath> _aftermaths;

    public Treasues(string sid, IReadOnlyCollection<ICrisisAftermath> aftermaths)
    {
        Sid = new CrisisSid(sid);
        _aftermaths = aftermaths;
    }

    public CrisisSid Sid { get; }

    public EventType EventType => EventType.Treasues;

    public IReadOnlyCollection<ICrisisAftermath> GetItems()
    {
        return _aftermaths;
    }
}