using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;

using CombatDicesTeam.Dices;

namespace Core.Crises;

internal sealed class RestoreAllButSingleRandomNotOptionAftermath : DialogueOptionAftermathBase
{
    private const int HEAL = 2;
    private readonly IDice _dice;
    private string[]? _selectedHeroesToHeal;

    public RestoreAllButSingleRandomNotOptionAftermath(IDice dice)
    {
        _dice = dice;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        DefineHeroesToHeal(aftermathContext);

        foreach (var hero in _selectedHeroesToHeal!)
        {
            aftermathContext.RestHero(hero, HEAL);
        }
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        DefineHeroesToHeal(aftermathContext);

        return new[]
        {
            (object)_selectedHeroesToHeal!,
            HEAL
        };
    }

    private void DefineHeroesToHeal(CampaignAftermathContext context)
    {
        var heroes = context.GetPartyHeroes();
        var rolledHero = _dice.RollFromList(heroes.ToArray());

        var selectedHeroes = new List<string>();

        foreach (var hero in heroes)
        {
            if (hero == rolledHero)
            {
                selectedHeroes.Add(hero);
            }
        }

        _selectedHeroesToHeal = selectedHeroes.ToArray();
    }
}