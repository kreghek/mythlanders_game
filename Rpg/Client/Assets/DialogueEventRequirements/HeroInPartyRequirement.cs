using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client.Assets.DialogueEventRequirements;

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