using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class HasHeroParagraphCondition : IDialogueParagraphCondition<ParagraphConditionContext>
{
    private readonly IDialogueSpeaker _hero;

    public HasHeroParagraphCondition(IDialogueSpeaker hero)
    {
        _hero = hero;
    }

    public bool Check(ParagraphConditionContext context)
    {
        return context.CurrentHeroes.Contains(_hero.ToString().ToLower());
    }
}

internal sealed class DisabledParagraphCondition : IDialogueParagraphCondition<ParagraphConditionContext>
{
    public bool Check(ParagraphConditionContext context)
    {
        return false;
    }
}

internal sealed class PreHistoryDisabledParagraphCondition : IDialogueParagraphCondition<PreHistoryConditionContext>
{
    public bool Check(PreHistoryConditionContext context)
    {
        return false;
    }
}