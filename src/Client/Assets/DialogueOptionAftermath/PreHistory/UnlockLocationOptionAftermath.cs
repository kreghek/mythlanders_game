using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.GameScreens.PreHistory;

namespace Client.Assets.DialogueOptionAftermath.PreHistory;

internal sealed class UnlockLocationOptionAftermath : PreHistoryDialogueOptionAftermathBase
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

    public override void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.UnlockLocation(_locationSid);
    }

    protected override IReadOnlyList<object> GetDescriptionValues(PreHistoryAftermathContext aftermathContext)
    {
        return new object[]
        {
            _locationSid
        };
    }
}