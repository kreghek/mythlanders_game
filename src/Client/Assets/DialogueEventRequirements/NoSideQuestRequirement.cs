using System.Linq;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class NoSideQuestRequirement : IDialogueEventRequirement
{
    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        return !context.ActiveStories.Any();
    }
}