using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

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