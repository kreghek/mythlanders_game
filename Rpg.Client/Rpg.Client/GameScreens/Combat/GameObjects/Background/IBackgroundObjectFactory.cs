using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal interface IBackgroundObjectFactory
    {
        IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects();

        IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects();
    }
}