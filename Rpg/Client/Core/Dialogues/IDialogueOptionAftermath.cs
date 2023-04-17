using Client.Core.Dialogues;

namespace Rpg.Client.Core.Dialogues
{
    internal interface IDialogueOptionAftermath
    {
        void Apply(IEventContext dialogContext);
    }
}