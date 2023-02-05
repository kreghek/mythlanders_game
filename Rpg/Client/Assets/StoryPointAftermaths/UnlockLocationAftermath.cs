using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPointAftermaths
{
    internal sealed class UnlockLocationAftermath : IStoryPointAftermath
    {
        private readonly GlobeNode _location;

        public UnlockLocationAftermath(GlobeNode location)
        {
            _location = location;
        }

        public void Apply()
        {
            _location.IsAvailable = true;
        }
    }
}