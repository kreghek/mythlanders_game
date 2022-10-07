using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.DialogueEventRequirements
{
    internal sealed class RequiredGlobeProgressEventRequirement : ITextEventRequirement
    {
        private readonly int _progress;

        public RequiredGlobeProgressEventRequirement(int progress)
        {
            _progress = progress;
        }

        public bool IsApplicableFor(Globe globe, GlobeNode targetNode)
        {
            return globe.GlobeLevel.Level >= _progress;
        }
    }
}