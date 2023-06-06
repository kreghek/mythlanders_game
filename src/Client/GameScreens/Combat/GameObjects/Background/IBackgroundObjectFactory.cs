using System.Collections.Generic;

namespace Client.GameScreens.Combat.GameObjects.Background;

internal interface IBackgroundObjectFactory
{
    IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects();

    IReadOnlyList<IBackgroundObject> CreateConnectorLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }

    IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }

    IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects();

    IReadOnlyList<IBackgroundObject> CreateMainLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }
}