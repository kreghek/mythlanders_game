using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueOptionAftermath;

[UsedImplicitly]
internal class RemoveResourceOptionAftermath: DialogueOptionAftermathBase
{
    private readonly string _resourceSid;
    private readonly int _count;

    public RemoveResourceOptionAftermath(string resourceSid, int count)
    {
        _resourceSid = resourceSid;
        _count = count;
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new object[]
        {
            GameObjectHelper.GetLocalizedProp(_resourceSid),
            _count
        };
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.RemoveResource(new Resource(new PropScheme(_resourceSid), _count));
    }
}