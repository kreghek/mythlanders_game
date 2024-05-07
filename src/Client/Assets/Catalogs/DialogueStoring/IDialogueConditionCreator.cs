using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueConditionCreator<in TParagraphConditionContext>
{
    IDialogueParagraphCondition<TParagraphConditionContext> Create(string conditionTypeSid, string data);
}