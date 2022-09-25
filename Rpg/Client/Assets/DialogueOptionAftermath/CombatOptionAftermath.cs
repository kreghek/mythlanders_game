using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal class CombatOptionAftermath : IOptionAftermath
    {
        private readonly string _sid;

        public CombatOptionAftermath(string sid)
        {
            _sid = sid;
        }

        public void Apply(IEventContext dialogContext)
        {
            dialogContext.StartCombat(_sid);
        }
    }
}