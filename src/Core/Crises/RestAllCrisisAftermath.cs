namespace Core.Crises;

public sealed class RestAllCrisisAftermath : ICrisisAftermath
{
    public RestAllCrisisAftermath(string sid)
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
            context.RestHero(hero, 1);
        }
    }
}