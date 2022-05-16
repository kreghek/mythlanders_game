using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPoints
{
    internal sealed class UnlockLocation: IStoryPointAftermath
    {
        private readonly GlobeNode _location;

        public UnlockLocation(GlobeNode location)
        {
            _location = location;
        }

        public void Apply()
        {
            _location.IsAvailable = true;
        }
    }
}