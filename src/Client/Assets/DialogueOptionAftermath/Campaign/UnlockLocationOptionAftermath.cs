using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

namespace Client.Assets.DialogueOptionAftermath.Campaign;

internal sealed class UnlockLocationOptionAftermath : CampaignDialogueOptionAftermathBase
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

    public override void Apply(CampaignAftermathContext aftermathContext)
    {
        aftermathContext.UnlockLocation(_locationSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(CampaignAftermathContext aftermathContext)
    {
        return new object[]
        {
            _locationSid
        };
    }
}