using CombatDicesTeam.Dices;

namespace Core.Crises;

public sealed class RestAllButSingleRandomNotCrisisAftermath : ICrisisAftermath
{
    private readonly IDice _dice;

    public RestAllButSingleRandomNotCrisisAftermath(string sid, IDice dice)
    {
        Sid = new CrisisAftermathSid(sid);
        _dice = dice;
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

        var rolledHero = _dice.RollFromList(heroes.ToArray());

        foreach (var hero in heroes)
        {
            if (hero == rolledHero)
            {
                context.RestHero(rolledHero, 2);
            }
        }
    }
}