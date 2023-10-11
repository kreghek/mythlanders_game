using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueOptionAftermath;

[UsedImplicitly]
internal class TakeExpoOptionAftermath: DialogueOptionAftermathBase
{
    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return ArraySegment<object>.Empty;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.AddResources(new Resource(new PropScheme("expo"), 10));
    }
}