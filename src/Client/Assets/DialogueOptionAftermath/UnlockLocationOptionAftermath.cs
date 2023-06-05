using Client.Core;
using Client.Core.Dialogues;

using Rpg.Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnlockLocationOptionAftermath : IDialogueOptionAftermath
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
}