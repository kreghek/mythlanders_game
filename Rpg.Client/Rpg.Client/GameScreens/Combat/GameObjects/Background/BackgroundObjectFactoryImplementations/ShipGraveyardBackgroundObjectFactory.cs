using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class ShipGraveyardBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private readonly IDice _dice;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public ShipGraveyardBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
            _dice = dice;
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
                        _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(BackgroundType.ChineseMonastery,
                            BackgroundLayerType.Far, 0),
                        position,
                        sourceRectangle,
                        new Vector2(0.5f, 1));
                    list.Add(houseObject);
                }
            }

            return list;
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

        public IReadOnlyList<IBackgroundObject> CreateMainLayerObjects()
        {
            var mainLayerObjects = new List<IBackgroundObject>();

            const int FLOOR_COUNT = 1024 / 256;

            for (var objectIndex = 0; objectIndex < FLOOR_COUNT; objectIndex++)
            {
                var isPassableArea = objectIndex == 0 || objectIndex == 3;
                var objects = CreateAreaMainObjects(isPassableArea);
                
                var sourceRectangle = new Rectangle(0, 0, 256, 256);

                if (_dice.RollD100() < 25)
                {
                    const int SPRITE_COUNT = 3;
                    const int COL_COUNT = 2;

                    var objectSpriteIndex = _dice.Roll(2, SPRITE_COUNT) - 1;

                    var col = objectSpriteIndex % COL_COUNT;
                    var row = objectSpriteIndex / COL_COUNT;

                    sourceRectangle = new Rectangle(col * 256, row * 256, 256, 256);
                }

                var topPosition = 180;

                var position = new Vector2(objectIndex * 256, topPosition);

                var floorObject = new PositionalStaticObject(
                    _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(BackgroundType.ChineseMonastery,
                        BackgroundLayerType.Main, 0),
                    position,
                    sourceRectangle,
                    new Vector2(0, 0));

                mainLayerObjects.Add(floorObject);
            }

            return mainLayerObjects;
        }

        private class BgMainObjectScheme
        {
            public BgMainObjectSchemeSize Size { get; set; }
            public bool IsPassable { get; set; }
            
            public (BackgroundType Location, BackgroundLayerType Layer, int SpritesheetIndex) Texture { get; set; }
            public Rectangle SourceRect { get; set; }

            public IBackgroundObject Create(GameObjectContentStorage gameObjectContentStorage)
            {
                return new PositionalStaticObject(gameObjectContentStorage.GetCombatBackgroundObjectsTexture(
                    Texture.Location,
                    Texture.Layer, Texture.SpritesheetIndex));
            }
        }

        private enum BgMainObjectSchemeSize
        {
            Size256,
            Size64
        }
        
        private IReadOnlyCollection<IBackgroundObject> CreateAreaMainObjects(bool isPassableArea)
        {
            if (!CheckRoll100Passed(25))
            {
                return ArraySegment<IBackgroundObject>.Empty;
            }

            var objectSchemes = GetObjectSchemes();

            foreach (var scheme in objectSchemes)
            {
                
            }

            {
                const int SPRITE_COUNT = 3;
                const int COL_COUNT = 2;

                var objectSpriteIndex = _dice.Roll(2, SPRITE_COUNT) - 1;

                var col = objectSpriteIndex % COL_COUNT;
                var row = objectSpriteIndex / COL_COUNT;

                sourceRectangle = new Rectangle(col * 256, row * 256, 256, 256);
            }
        }

        private static IReadOnlyCollection<BgMainObjectScheme> _bgObjectSchemes = new[]
        {
            new BgMainObjectScheme
            {
                
            }
        };
        
        private IReadOnlyCollection<BgMainObjectScheme> GetObjectSchemes()
        {
            throw new NotImplementedException();
        }

        private bool CheckRoll100Passed(int p)
        {
            return _dice.RollD100() < p;
        }

        public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        public IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
        {
            return CreateHouses(farLayerBottom: 180);
        }
    }
}