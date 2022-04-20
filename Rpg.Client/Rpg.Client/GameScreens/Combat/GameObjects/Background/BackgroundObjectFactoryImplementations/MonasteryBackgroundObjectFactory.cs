using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class MonasteryBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IDice _dice;

        public MonasteryBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
            _dice = dice;
        }

        public IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects()
        {
            var list = new List<IBackgroundObject>();

            for (var i = 0; i < 100; i++)
            {
                var weatherObject = new DemonicFireAnimatedObject(_gameObjectContentStorage.GetParticlesTexture(),
                    new Rectangle(0, 32, 32, 32),
                    new Vector2(i * 10, 240));
                list.Add(weatherObject);
            }

            return list;
        }

        public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        public IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
        {
            return CreateHouses(farLayerBottom: 180);
        }

        private IReadOnlyList<IBackgroundObject> CreateHouses(int farLayerBottom)
        {
            var list = new List<IBackgroundObject>();

            const int HOUSE_COUNT = 4 * 4;
            var indeces = _dice.RollFromList(Enumerable.Range(0, HOUSE_COUNT).ToArray(), HOUSE_COUNT).ToArray();

            for (var i = 0; i < HOUSE_COUNT; i++)
            {
                var objectIndex = indeces[i];
                if (_dice.RollD100() > 25)
                {
                    const int SPRITE_COUNT = 9;
                    const int COL_COUNT = 3;

                    var objectSpriteIndex = _dice.Roll(SPRITE_COUNT) - 1;
                    var position = new Vector2(objectIndex * 64, farLayerBottom);
                    var col = objectSpriteIndex % COL_COUNT;
                    var row = objectSpriteIndex / COL_COUNT;
                    var sourceRectangle = new Rectangle(col * 256, row * 256, 256, 256);
                    var houseObject = new PositionalStaticObject(
                        _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(BackgroundType.ChineseMonastery, BackgroundLayerType.Far, 0),
                        position,
                        sourceRectangle,
                        new Vector2(0.5f, 1));
                    list.Add(houseObject);
                }
            }

            return list;
        }
    }
}