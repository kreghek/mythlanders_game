using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPoints
{
    internal sealed class AddActivateStoryPoint : IStoryPointAftermath
    {
        private readonly IStoryPoint _newStoryPoint;
        private readonly Globe _globe;

        public AddActivateStoryPoint(IStoryPoint newStoryPoint, Globe globe)
        {
            _newStoryPoint = newStoryPoint;
            _globe = globe;
        }

        public void Apply()
        {
            _globe.AddActiveStoryPoint(_newStoryPoint);
        }
    }
}