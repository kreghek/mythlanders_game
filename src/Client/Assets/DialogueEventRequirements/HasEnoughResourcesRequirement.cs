using Client.Assets.Catalogs.Dialogues;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueEventRequirements;

[UsedImplicitly]
internal sealed class HasEnoughResourcesRequirement : IDialogueEventRequirement
{
    private readonly int _minimalAmount;
    private readonly string _resourceSid;

    public HasEnoughResourcesRequirement(string resourceSid, int minimalAmount)
    {
        _resourceSid = resourceSid;
        _minimalAmount = minimalAmount;
    }

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        return context.HasResource(new PropScheme(_resourceSid), _minimalAmount);
    }
}