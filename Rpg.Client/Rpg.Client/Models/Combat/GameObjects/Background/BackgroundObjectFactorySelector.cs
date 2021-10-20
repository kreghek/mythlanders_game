using Rpg.Client.Core;
using Rpg.Client.Models.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

namespace Rpg.Client.Models.Combat.GameObjects.Background
{
    internal sealed class BackgroundObjectFactorySelector
    {
        private GameObjectContentStorage _gameObjectContentStorage;
        private IBackgroundObjectFactory _backgroundObjectFactory;
        private EmptyBackgroundObjectFactory _empty;

        public void Initialize(GameObjectContentStorage gameObjectContentStorage)
        {
            _gameObjectContentStorage = gameObjectContentStorage;

            _backgroundObjectFactory = new ThicketBackgroundObjectFactory(_gameObjectContentStorage);
            _empty = new EmptyBackgroundObjectFactory();
        }

        public IBackgroundObjectFactory GetBackgroundObjectFactory(GlobeNodeSid nodeSid)
        {
            if (nodeSid == GlobeNodeSid.Thicket)
            {
                return _backgroundObjectFactory;
            }
            else
            {
                return _empty;
            }
        }
    }
}