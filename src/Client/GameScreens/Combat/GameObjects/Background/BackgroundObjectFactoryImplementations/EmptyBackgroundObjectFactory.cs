using System.Collections.Generic;

namespace Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

internal sealed class EmptyBackgroundObjectFactory : IBackgroundObjectFactory
{
    public IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }

    public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }
}