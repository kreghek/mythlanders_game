namespace Client.Core.Dialogues;

internal interface IDialogueEventRequirement
{
    bool IsApplicableFor(IDialogueEventRequirementContext context);
}