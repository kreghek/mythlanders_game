using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal interface IBackgroundObjectFactory
    {
        IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects();
        IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects();
    }
}