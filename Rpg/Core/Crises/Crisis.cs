namespace Core.Crises;

public sealed class Crisis : ICrisis
{
    private readonly IReadOnlyCollection<ICrisisAftermath> _aftermaths;

    public Crisis(string sid, IReadOnlyCollection<ICrisisAftermath> aftermaths)
    {
        Sid = new CrisisSid(sid);
        _aftermaths = aftermaths;
    }

    public CrisisSid Sid { get; }
    public IReadOnlyCollection<ICrisisAftermath> GetItems() => _aftermaths;
}