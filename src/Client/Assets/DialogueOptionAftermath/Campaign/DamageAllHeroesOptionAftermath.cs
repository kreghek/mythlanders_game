using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class DamageAllHeroesOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private const int DAMAGE = 1;

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        if (!heroes.Any())
        {
            // This is not normal.
            // if all heroes was defeat then the campaign must be interrupted.
        }

        foreach (var hero in heroes)
        {
            aftermathContext.DamageHero(hero, DAMAGE);
        }
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        return new object[]
        {
            heroes,
            DAMAGE
        };
    }
}