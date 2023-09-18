using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class PredefinedCombatOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly string _sid;

    public PredefinedCombatOptionAftermath(string sid)
    {
        _sid = sid;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.StartCombat(_sid);
    }
}