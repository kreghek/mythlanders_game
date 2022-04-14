using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class BackgroundObjectFactorySelector
    {
        private IBackgroundObjectFactory _backgroundObjectFactory;
        private EmptyBackgroundObjectFactory _empty;
        private GameObjectContentStorage _gameObjectContentStorage;
        private MonasteryBackgroundObjectFactory _monasteryObjectFactory;
        private ThicketBackgroundObjectFactory _thicketOjectFactory;

        public IBackgroundObjectFactory GetBackgroundObjectFactory(GlobeNodeSid nodeSid)
        {
            if (nodeSid == GlobeNodeSid.Thicket)
            {
                return _thicketOjectFactory;
            }

            if (nodeSid == GlobeNodeSid.Monastery)
            {
                return _monasteryObjectFactory;
            }

            if (nodeSid == GlobeNodeSid.Battleground)
            {
                return _backgroundObjectFactory;
            }

            return _empty;
        }

        public void Initialize(GameObjectContentStorage gameObjectContentStorage, BackgroundObjectCatalog backgroundObjectCatalog, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;

            _backgroundObjectFactory = new BattlegroundtBackgroundObjectFactory(_gameObjectContentStorage);
            _thicketOjectFactory = new ThicketBackgroundObjectFactory(_gameObjectContentStorage, backgroundObjectCatalog, dice);
            _monasteryObjectFactory = new MonasteryBackgroundObjectFactory(_gameObjectContentStorage);
            _empty = new EmptyBackgroundObjectFactory();
        }
    }
}