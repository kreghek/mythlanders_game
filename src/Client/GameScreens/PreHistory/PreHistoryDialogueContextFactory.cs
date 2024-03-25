using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryDialogueContextFactory: IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext>
{
    private readonly PreHistoryAftermathContext _aftermathContext;
    private readonly Player _player;

    public PreHistoryDialogueContextFactory(PreHistoryAftermathContext aftermathContext, Player player)
    {
        _aftermathContext = aftermathContext;
        _player = player;
    }
    
    public PreHistoryAftermathContext CreateAftermathContext()
    {
        return _aftermathContext;
    }

    public ParagraphConditionContext CreateParagraphConditionContext()
    {
        return new ParagraphConditionContext(_player);
    }
}