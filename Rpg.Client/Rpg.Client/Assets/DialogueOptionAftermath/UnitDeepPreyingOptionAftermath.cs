using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Core.GlobalEvents;

namespace Rpg.Client.Assets.DialogueOptionAftermath
{
    internal sealed class UnitDeepPreyingOptionAftermath : IOptionAftermath
    {
        private readonly UnitName _name;

        public UnitDeepPreyingOptionAftermath(UnitName name)
        {
            _name = name;
        }

        public void Apply(IEventContext dialogContext)
        {
            var globalEvent = new CharacterDeepPreyingGlobeEvent(_name);

            dialogContext.AddNewGlobalEvent(globalEvent);
        }
    }
}