using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal sealed class BackgroundObjectFactorySelector
    {
        private IBackgroundObjectFactory _battlegroundObjectFactory;
        private EmptyBackgroundObjectFactory _empty;
        private GameObjectContentStorage _gameObjectContentStorage;
        private MonasteryBackgroundObjectFactory _monasteryObjectFactory;
        private ShipGraveyardBackgroundObjectFactory _shipGraveyardObjectFactory;
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
                return _battlegroundObjectFactory;
            }

            if (nodeSid == GlobeNodeSid.ShipGraveyard)
            {
                return _shipGraveyardObjectFactory;
            }

            return _empty;
        }

        public void Initialize(GameObjectContentStorage gameObjectContentStorage,
            BackgroundObjectCatalog backgroundObjectCatalog, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;

            _battlegroundObjectFactory = new BattlegroundBackgroundObjectFactory(_gameObjectContentStorage);
            _thicketOjectFactory = new ThicketBackgroundObjectFactory(_gameObjectContentStorage);
            _monasteryObjectFactory = new MonasteryBackgroundObjectFactory(_gameObjectContentStorage, dice);
            _shipGraveyardObjectFactory = new ShipGraveyardBackgroundObjectFactory(_gameObjectContentStorage, dice);
            _empty = new EmptyBackgroundObjectFactory();
        }
    }
}