using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class
    PreHistoryDialogueContextFactory : IDialogueContextFactory<PreHistoryConditionContext, PreHistoryAftermathContext>
{
    private readonly PreHistoryAftermathContext _aftermathContext;

    public PreHistoryDialogueContextFactory(PreHistoryAftermathContext aftermathContext, Player player)
    {
        _aftermathContext = aftermathContext;
    }

    public PreHistoryAftermathContext CreateAftermathContext()
    {
        return _aftermathContext;
    }

    public PreHistoryConditionContext CreateParagraphConditionContext()
    {
        return new PreHistoryConditionContext();
    }
}