using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnitDeepPreyingOptionAftermath : IDialogueOptionAftermath<AftermathContext>
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

    public void Apply(AftermathContext aftermathContext)
    {
        var globalEvent = new CharacterDeepPreyingGlobeEvent(_name);

        aftermathContext.AddNewGlobalEvent(globalEvent);
    }
}