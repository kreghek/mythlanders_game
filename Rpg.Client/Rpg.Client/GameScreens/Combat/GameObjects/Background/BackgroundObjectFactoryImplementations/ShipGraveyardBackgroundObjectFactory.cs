using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

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

            const int AREA_COUNT = 1024 / 256;

            for (var areaIndex = 0; areaIndex < AREA_COUNT; areaIndex++)
            {
                var isPassableArea = !(areaIndex == 0 || areaIndex == 3);
                var topPosition = 180;
                var position = new Vector2(areaIndex * 256, topPosition);

                var objects = CreateAreaMainObjects(isPassableArea, position);

                mainLayerObjects.AddRange(objects);
            }

            return mainLayerObjects;
        }

        private class BgMainObjectScheme
        {
            public BgMainObjectSchemeSize Size { get; set; }
            public bool IsPassable { get; set; }
            
            public (BackgroundType Location, BackgroundLayerType Layer, int SpritesheetIndex) Texture { get; set; }
            public Rectangle SourceRect { get; set; }
            public Vector2 Origin { get; set; }

            public IBackgroundObject Create(GameObjectContentStorage gameObjectContentStorage, Vector2 position)
            {
                return new PositionalStaticObject(gameObjectContentStorage.GetCombatBackgroundObjectsTexture(
                    Texture.Location,
                    Texture.Layer, Texture.SpritesheetIndex), position, SourceRect, Origin);
            }
        }

        private enum BgMainObjectSchemeSize
        {
            Size256,
            Size64
        }
        
        private IReadOnlyCollection<IBackgroundObject> CreateAreaMainObjects(bool isPassableArea, Vector2 position)
        {
            var objectSchemes = GetObjectSchemes();
            var objList = new List<IBackgroundObject>();

            var largeObjSchemesOpenList = objectSchemes.Where(x => x.Size == BgMainObjectSchemeSize.Size256 && ((!isPassableArea) || (isPassableArea && x.IsPassable))).ToList();
            var smallObjSchemesOpenList = objectSchemes.Where(x => x.Size == BgMainObjectSchemeSize.Size64 && ((!isPassableArea) || (isPassableArea && x.IsPassable))).ToList();

            if (largeObjSchemesOpenList.Any())
            {
                var isLargeObj = CheckRoll100Passed(50);

                if (isLargeObj)
                {
                    var scheme = _dice.RollFromList(largeObjSchemesOpenList);

                    var obj = scheme.Create(_gameObjectContentStorage, position);
                    objList.Add(obj);
                    largeObjSchemesOpenList.Remove(scheme);
                }
                else
                {
                    CreateSmallObjects(position, objList, smallObjSchemesOpenList);
                }
            }
            else
            {
                CreateSmallObjects(position, objList, smallObjSchemesOpenList);
            }

            return objList;
        }

        private void CreateSmallObjects(Vector2 areaPosition, List<IBackgroundObject> objList, List<BgMainObjectScheme> smallObjSchemesOpenList)
        {
            if (!smallObjSchemesOpenList.Any())
            {
                return;
            }

            const int MAX_SMALL_OBJS_IN_AREA = 4;
            var rollCount = Math.Min(MAX_SMALL_OBJS_IN_AREA, smallObjSchemesOpenList.Count);
            var objSchemes = _dice.RollFromList(smallObjSchemesOpenList, rollCount).ToArray();
            var positionIndecies = _dice.ShuffleList(new[] { 0, 1, 2, 3 });

            for (int i = 0; i < objSchemes.Length; i++)
            {
                const int AREA_SMALL_OBJ_ROW_COUNT = 2;

                if (!smallObjSchemesOpenList.Any())
                {
                    break;
                }

                var positionIndex = positionIndecies[i];
                var areaCol = positionIndex % AREA_SMALL_OBJ_ROW_COUNT;
                var areaRow = positionIndex / AREA_SMALL_OBJ_ROW_COUNT;

                var scheme = objSchemes[i];
                var obj = scheme.Create(_gameObjectContentStorage, areaPosition + new Vector2(areaCol * 64, areaRow * 64));

                objList.Add(obj);
                smallObjSchemesOpenList.Remove(scheme);
            }
        }

        private static IReadOnlyCollection<BgMainObjectScheme> _bgObjectSchemes = new[]
        {
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(0,0, 256, 256),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(256, 0, 256, 256),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(0, 256, 256, 256),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(256, 256, 256, 256),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },

            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(0, 0, 64, 64),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(1 * 64, 0, 64, 64),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(0 * 64, 1 * 64, 64, 64),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(1 * 64, 1 * 64, 64, 64),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = new Rectangle(0 * 64, 2 * 64, 64, 64),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
        };
        
        private IReadOnlyCollection<BgMainObjectScheme> GetObjectSchemes()
        {
            return _bgObjectSchemes;
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
            return new List<IBackgroundObject>(0);
        }
    }
}