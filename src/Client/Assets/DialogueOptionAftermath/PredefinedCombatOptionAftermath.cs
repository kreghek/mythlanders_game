using Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class PredefinedCombatOptionAftermath : IDialogueOptionAftermath
{
    private readonly string _sid;

    public PredefinedCombatOptionAftermath(string sid)
    {
        _sid = sid;
    }

    public void Apply(IEventContext dialogContext)
    {
        dialogContext.StartCombat(_sid);
    }
}