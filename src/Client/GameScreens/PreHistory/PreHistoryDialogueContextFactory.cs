using System;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryDialogueContextFactory: IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext>
{
    private readonly PreHistoryAftermathContext _aftermathContext;

    public PreHistoryDialogueContextFactory(PreHistoryAftermathContext aftermathContext)
    {
        _aftermathContext = aftermathContext;
    }
    
    public PreHistoryAftermathContext CreateAftermathContext()
    {
        return _aftermathContext;
    }

    public ParagraphConditionContext CreateParagraphConditionContext()
    {
        throw new NotImplementedException();
    }
}