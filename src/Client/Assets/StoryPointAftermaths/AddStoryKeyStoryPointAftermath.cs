using Client.Core;

namespace Client.Assets.StoryPointAftermaths;

internal sealed class AddStoryKeyStoryPointAftermath: IStoryPointAftermath
{
    private readonly string _key;
    private readonly Globe _globe;

    public AddStoryKeyStoryPointAftermath(string key, Globe globe)
    {
        _key = key;
        _globe = globe;
    }

    public void Apply()
    {
        _globe.Player.StoryState.AddKey(_key);
    }
}