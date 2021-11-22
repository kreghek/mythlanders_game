using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class BackgroundObjectFactorySelector
    {
        private IBackgroundObjectFactory _backgroundObjectFactory;
        private EmptyBackgroundObjectFactory _empty;
        private GameObjectContentStorage _gameObjectContentStorage;

        public IBackgroundObjectFactory GetBackgroundObjectFactory(GlobeNodeSid nodeSid)
        {
            if (nodeSid == GlobeNodeSid.Battleground)
            {
                return _backgroundObjectFactory;
            }

            return _empty;
        }

        public void Initialize(GameObjectContentStorage gameObjectContentStorage)
        {
            _gameObjectContentStorage = gameObjectContentStorage;

            _backgroundObjectFactory = new ThicketBackgroundObjectFactory(_gameObjectContentStorage);
            _empty = new EmptyBackgroundObjectFactory();
        }
    }
}