using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnlockLocationOptionAftermath : DialogueOptionAftermathBase
{
    private readonly ILocationSid _locationSid;

    public UnlockLocationOptionAftermath(ILocationSid locationSid)
    {
        _locationSid = locationSid;
    }

    public void Apply(IEventContext dialogContext)
    {
        dialogContext.UnlockLocation(_locationSid);
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.UnlockLocation(_locationSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return new object[]
        {
            _locationSid
        };
    }
}