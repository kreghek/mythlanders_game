using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnlockLocationOptionAftermath : IDialogueOptionAftermath<AftermathContext>
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

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.UnlockLocation(_locationSid);
    }
}