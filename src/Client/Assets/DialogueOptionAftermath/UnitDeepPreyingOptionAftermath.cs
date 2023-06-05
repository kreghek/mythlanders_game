using Client.Core.Dialogues;

using Rpg.Client.Assets.GlobalEffects;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnitDeepPreyingOptionAftermath : IDialogueOptionAftermath
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