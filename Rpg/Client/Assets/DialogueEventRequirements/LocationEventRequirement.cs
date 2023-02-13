using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueEventRequirements
{
    internal sealed class LocationEventRequirement : ITextEventRequirement
    {
        private readonly IReadOnlyCollection<GlobeNodeSid> _locationSids;

        public LocationEventRequirement(IReadOnlyCollection<GlobeNodeSid> locationSids)
        {
            _locationSids = locationSids;
        }

        public bool IsApplicableFor(Globe globe, GlobeNode targetNode)
        {
            return _locationSids.Contains(targetNode.Sid);
        }
    }
}