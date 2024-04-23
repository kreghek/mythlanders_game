using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnlockFeatureOptionAftermath : DialogueOptionAftermathBase
{
    private readonly GameFeature _feature;

    public UnlockFeatureOptionAftermath(GameFeature feature)
    {
        _feature = feature;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.UnlockFeature(_feature);
    }

    public static IDialogueOptionAftermath<CampaignAftermathContext> CreateFromData(string data)
    {
        var features = CatalogHelper.GetAllFromStaticCatalog<GameFeature>(typeof(GameFeatures));
        var feature = features.Single(x => x.Value == data);
        return new UnlockFeatureOptionAftermath(feature);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new object[]
        {
            _feature.Value
        };
    }
}