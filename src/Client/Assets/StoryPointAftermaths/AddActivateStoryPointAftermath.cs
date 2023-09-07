using Client.Core;

namespace Client.Assets.StoryPointAftermaths;

internal sealed class AddActivateStoryPointAftermath : IStoryPointAftermath
{
    private readonly Globe _globe;
    private readonly IStoryPoint _newStoryPoint;

    public AddActivateStoryPointAftermath(IStoryPoint newStoryPoint, Globe globe)
    {
        _newStoryPoint = newStoryPoint;
        _globe = globe;
    }

    public void Apply()
    {
        _globe.AddActiveStoryPoint(_newStoryPoint);
    }
}