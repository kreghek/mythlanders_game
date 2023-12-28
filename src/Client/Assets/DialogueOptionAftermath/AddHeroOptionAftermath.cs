using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Heroes;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class AddHeroOptionAftermath : DialogueOptionAftermathBase
{
    private readonly UnitScheme _scheme;

    public AddHeroOptionAftermath(UnitScheme scheme)
    {
        _scheme = scheme;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        const int DEFAULT_LEVEL = 1;
        var unit = new Hero(_scheme, DEFAULT_LEVEL)
        {
            IsPlayerControlled = true
        };
        aftermathContext.AddNewHero(unit);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new object[]
        {
            _scheme.Name
        };
    }
}