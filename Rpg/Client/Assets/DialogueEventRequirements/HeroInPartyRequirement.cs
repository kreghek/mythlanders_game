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

    public bool IsApplicableFor(Globe globe, LocationSid targetLocation)
    {
        var heroes = globe.Player.Party.GetUnits().Select(x=>x.UnitScheme.Name);
        return _heroSids.All(x => heroes.Contains(x));
    }
}