﻿using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;

using CombatDicesTeam.Dices;

namespace Core.Crises;

internal sealed class RestSingleRandomOptionAftermath : DialogueOptionAftermathBase
{
    private const int HEAL = 2;
    private readonly IDice _dice;
    private string? _selectedHeroToHeal;

    public RestSingleRandomOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        DefineHeroToHealOrNothing(aftermathContext);

        aftermathContext.DamageHero(_selectedHeroToHeal!, HEAL);
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
}