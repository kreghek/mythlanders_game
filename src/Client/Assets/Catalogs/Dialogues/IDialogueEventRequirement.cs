namespace Client.Assets.Catalogs.Dialogues;

public interface IDialogueEventRequirement
{
    bool IsApplicableFor(IDialogueEventRequirementContext context);
}