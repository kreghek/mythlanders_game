namespace Core.Crises;

public sealed class Crisis : ICrisis
{
    private readonly IReadOnlyCollection<ICrisisAftermath> _aftermaths;

    public Crisis(string sid, IReadOnlyCollection<ICrisisAftermath> aftermaths)
    {
        Sid = new CrisisSid(sid);
        _aftermaths = aftermaths;
    }

    /// <summary>
    /// Symbolic identifier of event.
    /// </summary>
    public CrisisSid Sid { get; }

    /// <summary>
    /// Small event type.
    /// </summary>
    public EventType EventType => EventType.Crisis;

    /// <summary>
    /// Small item options.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<ICrisisAftermath> GetItems()
    {
        return _aftermaths;
    }
}