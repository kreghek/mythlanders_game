using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class RestAllOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private const int DEFAULT_HEAL = 1;
    private readonly int _value;

    public RestAllOptionAftermath(int value = DEFAULT_HEAL)
    {
        _value = value;
    }

    internal static IDialogueOptionAftermath<CampaignAftermathContext> CreateFromData(string data)
    {
        return new RestAllOptionAftermath(ParseData(data));
    }

    private static int ParseData(string data)
    {
        if (int.TryParse(data, out var value) && value > 0)
            return value;

        return DEFAULT_HEAL;
    }

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
            aftermathContext.RestHero(hero, _value);
        }
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        var heroes = aftermathContext.GetPartyHeroes();

        return new object[]
        {
            heroes,
            _value
        };
    }
}