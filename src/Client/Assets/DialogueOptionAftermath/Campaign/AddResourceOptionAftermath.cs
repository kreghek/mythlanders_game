using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

[UsedImplicitly]
internal class AddResourceOptionAftermath : CampaignDialogueOptionAftermathBase
{
    private readonly int _count;
    private readonly string _resourceSid;

    public AddResourceOptionAftermath(string resourceSid, int count)
    {
        _resourceSid = resourceSid;
        _count = count;
    }

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.AddResources(new Resource(new PropScheme(_resourceSid), _count));
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new object[]
        {
            GameObjectHelper.GetLocalizedProp(_resourceSid),
            _count
        };
    }
}