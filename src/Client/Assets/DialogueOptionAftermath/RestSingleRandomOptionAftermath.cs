using Client.Assets.Catalogs.Dialogues;
using System.Collections.Generic;

using CombatDicesTeam.Dices;
using Client.Assets.DialogueOptionAftermath;
using System.Linq;

namespace Core.Crises;

internal sealed class RestSingleRandomOptionAftermath : DialogueOptionAftermathBase
{
    private readonly IDice _dice;
    private string? _selectedHeroToHeal;

    const int HEAL = 2;

    public RestSingleRandomOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        DefineHeroToHealOrNothing(aftermathContext);

        aftermathContext.DamageHero(_selectedHeroToHeal!, HEAL);
    }

    private void DefineHeroToHealOrNothing(AftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        if (!heroes.Any())
        {
            // This is not normal.
            // if all heroes was defeat then the campaign must be interrupted.
        }

        var rolledHero = _dice.RollFromList(heroes.ToArray());
        _selectedHeroToHeal = rolledHero;
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        DefineHeroToHealOrNothing(aftermathContext);

        return new object[]
        {
            _selectedHeroToHeal!,
            HEAL
        };
    }
}