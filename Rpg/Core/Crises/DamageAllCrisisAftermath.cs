namespace Core.Crises;

public sealed class DamageAllCrisisAftermath : ICrisisAftermath
{
    public DamageAllCrisisAftermath(string sid)
    {
        Sid = new CrisisAftermathSid(sid);
    }
    public CrisisAftermathSid Sid { get; }
    public void Apply(ICrisisAftermathContext context)
    {
        throw new NotImplementedException();
    }
}