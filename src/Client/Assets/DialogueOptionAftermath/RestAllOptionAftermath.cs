using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;

namespace Core.Crises;

internal sealed class RestAllOptionAftermath : DialogueOptionAftermathBase
{
    private const int HEAL = 1;


    public override void Apply(AftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        if (!heroes.Any())
        {
            // This is not normal.
            // if all heroes was defeat then the campaign must be interrupted.
        }

        foreach (var hero in heroes)
        {
            aftermathContext.RestHero(hero, HEAL);
        }
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        return new object[]
        {
            heroes,
            HEAL
        };
    }
}