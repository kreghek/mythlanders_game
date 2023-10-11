using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using Core.Props;

using JetBrains.Annotations;

namespace Client.Assets.DialogueEventRequirements;

[UsedImplicitly]
internal sealed class HeroInPartyRequirement : IDialogueEventRequirement
{
    private readonly UnitName[] _heroSids;

    public HeroInPartyRequirement(params UnitName[] heroSids)
    {
        _heroSids = heroSids;
    }

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        var heroes = context.ActiveHeroesInParty;
        return _heroSids.All(x => heroes.Contains(x));
    }
}

internal sealed class HasEnoughResourcesRequirement : IDialogueEventRequirement
{
    private readonly string _resourceSid;
    private readonly int _minimalAmount;

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