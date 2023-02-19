using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal sealed class UnlockLocationOptionAftermath : IDialogueOptionAftermath
    {
        private readonly LocationSid _locationSid;

        public UnlockLocationOptionAftermath(LocationSid locationSid)
        {
            _locationSid = locationSid;
        }

        public void Apply(IEventContext dialogContext)
        {
            dialogContext.UnlockLocation(_locationSid);
        }
    }
}