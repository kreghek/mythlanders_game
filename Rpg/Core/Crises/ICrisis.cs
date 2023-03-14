namespace Core.Crises;

public interface ICrisis
{
    string Sid { get; }

    IReadOnlyCollection<ICrisisAftermath> GetPreItems() => ArraySegment<ICrisisAftermath>.Empty;
}

public interface ICrisisAftermath
{
    void Apply(ICrisisAftermathContext context);
}

public interface ICrisisAftermathContext
{
    
}