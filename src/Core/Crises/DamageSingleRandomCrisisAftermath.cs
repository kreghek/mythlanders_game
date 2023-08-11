using CombatDicesTeam.Dices;

namespace Core.Crises;

public sealed class DamageSingleRandomCrisisAftermath : ICrisisAftermath
{
    private readonly IDice _dice;

    public DamageSingleRandomCrisisAftermath(string sid, IDice dice)
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

        context.DamageHero(rolledHero, 3);
    }
}