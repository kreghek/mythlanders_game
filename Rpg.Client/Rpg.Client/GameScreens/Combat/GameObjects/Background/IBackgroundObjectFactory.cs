using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
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

        IReadOnlyList<IBackgroundObject> CreateMainLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects();
    }
}