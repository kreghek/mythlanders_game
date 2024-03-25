using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dices;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class RestSingleRandomOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private const int HEAL = 2;
    private readonly IDice _dice;
    private string? _selectedHeroToHeal;

    public RestSingleRandomOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        DefineHeroToHealOrNothing(aftermathContext);

        aftermathContext.DamageHero(_selectedHeroToHeal!, HEAL);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        DefineHeroToHealOrNothing(aftermathContext);

        return new object[]
        {
            _selectedHeroToHeal!,
            HEAL
        };
    }

    private void DefineHeroToHealOrNothing(CampaignAftermathContext aftermathContext)
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