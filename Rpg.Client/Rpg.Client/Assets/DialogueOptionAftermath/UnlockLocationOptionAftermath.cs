using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal sealed class UnlockLocationOptionAftermath: IOptionAftermath
    {
        private readonly GlobeNodeSid _locationSid;

        public UnlockLocationOptionAftermath(GlobeNodeSid locationSid)
        {
            _locationSid = locationSid;
        }

        public void Apply(IEventContext dialogContext)
        {
            dialogContext.UnlockLocation(_locationSid);
        }
    }
}