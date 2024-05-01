using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class DisabledToSelectRequirement : IDialogueEventRequirement
{
    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        return false;
    }
}