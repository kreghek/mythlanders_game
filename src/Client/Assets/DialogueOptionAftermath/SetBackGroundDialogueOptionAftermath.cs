using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class SetBackGroundDialogueOptionAftermath: IDialogueOptionAftermath<PreHistoryAftermathContext>
{
    private readonly string _backgroundName;
    public SetBackGroundDialogueOptionAftermath(string backgroundName) {
        _backgroundName = backgroundName;
    }

    public void Apply(PreHistoryAftermathContext aftermathContext)
    {
        aftermathContext.SetBackground(_backgroundName);
    }

    public string GetDescription(PreHistoryAftermathContext aftermathContext)
    {
        return string.Empty;
    }

    public bool IsHidden => true;
}