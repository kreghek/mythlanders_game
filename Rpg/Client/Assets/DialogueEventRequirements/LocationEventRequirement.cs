using System.Collections.Generic;
using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.DialogueEventRequirements
{
    internal sealed class LocationEventRequirement : ITextEventRequirement
    {
        private readonly IReadOnlyCollection<LocationSid> _locationSids;

        public LocationEventRequirement(IReadOnlyCollection<LocationSid> locationSids)
        {
            _locationSids = locationSids;
        }

        public bool IsApplicableFor(Globe globe, LocationSid targetLocation)
        {
            return _locationSids.Contains(targetLocation);
        }
    }
}