using System.Linq;

using Client.Assets.Catalogs.Dialogues;

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