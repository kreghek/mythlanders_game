using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens;

using CombatDicesTeam.Dialogues;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueOptionAftermath;

[UsedImplicitly]
internal class AddResourceOptionAftermath : DialogueOptionAftermathBase
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

    public static IDialogueOptionAftermath<CampaignAftermathContext> CreateFromData(string data)
    {
        var args = data.Split(' ');
        return new AddResourceOptionAftermath(args[0], int.Parse(args[1]));
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