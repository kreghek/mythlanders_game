using System.Linq;

using Client.Core.Dialogues;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class StoryKeyRequirement: IDialogueEventRequirement
{
    private readonly string[] _requiredKeys;

    public StoryKeyRequirement(params string[] keys)
    {
        _requiredKeys = keys;
    }

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        var activeKeys = context.DialogueKeys;
        return _requiredKeys.All(x => activeKeys.Contains(x));
    }
}