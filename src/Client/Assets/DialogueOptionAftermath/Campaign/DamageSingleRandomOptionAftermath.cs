using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dices;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class DamageSingleRandomOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private const int DAMAGE = 3;
    private readonly IDice _dice;
    private string? _selectedHeroToDamage;

    public DamageSingleRandomOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        DefineHeroToDamageOrNothing(aftermathContext);

        aftermathContext.DamageHero(_selectedHeroToDamage!, DAMAGE);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        DefineHeroToDamageOrNothing(aftermathContext);

        return new object[]
        {
            _selectedHeroToDamage!,
            DAMAGE
        };
    }

    private void DefineHeroToDamageOrNothing(CampaignAftermathContext aftermathContext)
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
}