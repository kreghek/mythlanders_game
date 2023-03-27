namespace Core.Crises;

public sealed class DamageSingleRandomCrisisAftermath : ICrisisAftermath
{
    public DamageSingleRandomCrisisAftermath(string sid)
    {
        Sid = new CrisisAftermathSid(sid);
    }
    public CrisisAftermathSid Sid { get; }
    public void Apply(ICrisisAftermathContext context)
    {
    }
}