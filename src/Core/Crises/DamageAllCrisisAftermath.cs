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
        var heroes = context.GetAvailableHeroes();

        if (!heroes.Any())
        {
            // This is not normal.
            // if all heroes was defeat the campaign must be interupted.
        }

        foreach (var hero in heroes)
        {
            context.DamageHero(hero, 1);
        }
    }
}