using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.DialogueEventRequirements
{
    internal sealed class RequiredGlobeProgressEventRequirement : IDialogueEventRequirement
    {
        private readonly int _progress;

        public RequiredGlobeProgressEventRequirement(int progress)
        {
            _progress = progress;
        }

        public bool IsApplicableFor(Globe globe, LocationSid targetLocation)
        {
            return globe.GlobeLevel.Level >= _progress;
        }
    }
}