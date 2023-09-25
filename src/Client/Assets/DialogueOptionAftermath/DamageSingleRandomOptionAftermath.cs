using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dices;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class DamageSingleRandomOptionAftermath : DialogueOptionAftermathBase
{
    private readonly IDice _dice;
    private string? _selectedHeroToDamage;
    
    const int DAMAGE = 3;

    public DamageSingleRandomOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        DefineHeroToDamageOrNothing(aftermathContext);

        aftermathContext.DamageHero(_selectedHeroToDamage!, DAMAGE);
    }

    private void DefineHeroToDamageOrNothing(AftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        if (!heroes.Any())
        {
            // This is not normal.
            // if all heroes was defeat then the campaign must be interrupted.
        }

        var rolledHero = _dice.RollFromList(heroes.ToArray());
        _selectedHeroToDamage = rolledHero;
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        DefineHeroToDamageOrNothing(aftermathContext);

        return new object[]
        {
            _selectedHeroToDamage!,
            DAMAGE
        };
    }
}