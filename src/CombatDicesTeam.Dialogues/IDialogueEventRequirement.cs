namespace Client.Core.Dialogues;

public interface IDialogueEventRequirement
{
    bool IsApplicableFor(IDialogueEventRequirementContext context);
}