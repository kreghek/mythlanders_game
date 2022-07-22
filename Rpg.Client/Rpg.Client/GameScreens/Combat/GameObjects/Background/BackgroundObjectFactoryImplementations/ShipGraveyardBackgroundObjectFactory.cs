using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class ShipGraveyardBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private static readonly IReadOnlyCollection<IBgMainObjectScheme> _bgObjectSchemes = new[]
        {
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 1),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 2),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 3),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 4),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 5),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 6),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size256,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size256, 7),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0)
            },

            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 1),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 2),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 3),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 4),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = true,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 5),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 6),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 7),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 8),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            },
            new BgMainObjectScheme
            {
                IsPassable = false,
                Size = BgMainObjectSchemeSize.Size64,
                Origin = Vector2.Zero,
                SourceRect = GetSourceRectOneBased(BgMainObjectSchemeSize.Size64, 9),
                Texture = new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1)
            }
        };

        private readonly IDice _dice;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly Vector2 _haribdaPosition = new Vector2(245, 90);

        public ShipGraveyardBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
            _dice = dice;
        }

        private bool CheckRoll100Passed(int p)
        {
            return _dice.RollD100() < p;
        }

        private IReadOnlyCollection<IBackgroundObject> CreateAreaMainObjects(bool isPassableArea, Vector2 position)
        {
            var objectSchemes = GetObjectSchemes();
            var objList = new List<IBackgroundObject>();

            var largeObjSchemesOpenList = objectSchemes.Where(x =>
                    x.Size == BgMainObjectSchemeSize.Size256 && ((!isPassableArea) || (isPassableArea && x.IsPassable)))
                .ToList();
            var smallObjSchemesOpenList = objectSchemes.Where(x =>
                    x.Size == BgMainObjectSchemeSize.Size64 && ((!isPassableArea) || (isPassableArea && x.IsPassable)))
                .ToList();

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

        private void CreateSmallObjects(Vector2 areaPosition, List<IBackgroundObject> targetObjList,
            List<IBgMainObjectScheme> openList)
        {
            if (!openList.Any())
            {
                return;
            }

            const int MAX_SMALL_OBJS_IN_AREA = 4;
            var rollCount = Math.Min(MAX_SMALL_OBJS_IN_AREA, openList.Count);
            var objSchemes = _dice.RollFromList(openList, rollCount).ToArray();
            var positionIndecies = _dice.ShuffleList(new[] { 0, 1, 2, 3 });

            for (var i = 0; i < objSchemes.Length; i++)
            {
                const int AREA_SMALL_OBJ_ROW_COUNT = 2;

                if (!openList.Any())
                {
                    break;
                }

                var positionIndex = positionIndecies[i];
                var areaCol = positionIndex % AREA_SMALL_OBJ_ROW_COUNT;
                var areaRow = positionIndex / AREA_SMALL_OBJ_ROW_COUNT;

                var scheme = objSchemes[i];
                var obj = scheme.Create(_gameObjectContentStorage,
                    areaPosition + new Vector2(areaCol * 64, areaRow * 64));

                targetObjList.Add(obj);
                openList.Remove(scheme);
            }
        }

        private IReadOnlyCollection<IBgMainObjectScheme> GetObjectSchemes()
        {
            return _bgObjectSchemes;
        }

        private static Rectangle GetSourceRect(BgMainObjectSchemeSize size, int index)
        {
            const int COL_COUNT_64 = 3;
            const int COL_COUNT_256 = 3;

            switch (size)
            {
                case BgMainObjectSchemeSize.Size256:
                    {
                        const int SIZE = 256;
                        var (x, y) = GetSourceRectCoords(index, COL_COUNT_256);
                        return new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
                    }

                case BgMainObjectSchemeSize.Size64:
                    {
                        const int SIZE = 64;
                        var (x, y) = GetSourceRectCoords(index, COL_COUNT_64);
                        return new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
                    }

                default:
                    throw new InvalidOperationException();
            }
        }

        private static (int x, int y) GetSourceRectCoords(int index, int colCount)
        {
            return new(index % colCount, index / colCount);
        }

        private static Rectangle GetSourceRectOneBased(BgMainObjectSchemeSize size, int indexOneBased)
        {
            return GetSourceRect(size, indexOneBased - 1);
        }

        public IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects()
        {
            return new[]
            {
                new HaribdaBackgroundObject(
                    _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(BackgroundType.GreekShipGraveyard,
                        BackgroundLayerType.Far, 0), _haribdaPosition)
            };
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

        public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        public IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
        {
            return ArraySegment<IBackgroundObject>.Empty;
        }

        private interface IBgMainObjectScheme
        {
            bool IsPassable { get; set; }
            BgMainObjectSchemeSize Size { get; set; }

            IBackgroundObject Create(GameObjectContentStorage gameObjectContentStorage, Vector2 position);
        }

        private class BgMainObjectScheme : IBgMainObjectScheme
        {
            public Vector2 Origin { get; set; }
            public Rectangle SourceRect { get; set; }

            public (BackgroundType Location, BackgroundLayerType Layer, int SpritesheetIndex) Texture { get; set; }
            public BgMainObjectSchemeSize Size { get; set; }
            public bool IsPassable { get; set; }

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

        private sealed class HaribdaBackgroundObject : IBackgroundObject
        {
            private readonly Vector2 _centerPosition;
            private readonly int[] _particleCounts = { 270, 180, 15 };

            private readonly ParticleSystem _particleSystem;
            private readonly float[] _radiuses = { 100f, 50f, 4f };

            private readonly double[] _ringCounters;
            private readonly double[] _ringSpeeds = { 0.0001, -0.0002, 0.00005 };
            private readonly Texture2D _texture;

            public HaribdaBackgroundObject(Texture2D texture, Vector2 centerPosition)
            {
                _texture = texture;
                _centerPosition = centerPosition;

                _ringCounters = new double[3];

                _particleSystem = new ParticleSystem(centerPosition, new MothParticleGenerator(new[] { texture }, 400));
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                for (var i = _ringCounters.Length - 1; i >= 0; i--)
                {
                    for (var j = 0; j < _particleCounts[i]; j++)
                    {
                        for (var k = 0; k < 2; k++)
                        {
                            var fullRound = Math.PI * 2d;
                            var arc = fullRound / _particleCounts[i];
                            var q = arc * j;

                            var radiusQ = ((i + 167 + j + 34) * 11313) % 100 / 100f;

                            var counter = _ringCounters[i] * (1 + k * 1.5f);
                            var position = new Vector2(
                                (float)(Math.Cos(counter + q) * (_radiuses[i] + (radiusQ * 50)) + _centerPosition.X),
                                (float)(Math.Sin(counter + q) * (_radiuses[i] + (radiusQ * 50)) + _centerPosition.Y));

                            const int COL_COUNT = 6;
                            const int SPRITE_COUNT = 6;

                            var spriteIndex = ((i + 167 + j + 34) * 1313) % SPRITE_COUNT;
                            var x = spriteIndex % COL_COUNT;
                            var y = spriteIndex / COL_COUNT;

                            var colorT = ((float)k) / 2;
                            var color = Color.Lerp(Color.White, Color.Transparent, colorT * 0.75f);
                            spriteBatch.Draw(_texture, position, new Rectangle(x * 32, y * 32, 32, 32), color);
                        }
                    }
                }

                _particleSystem.Draw(spriteBatch);
            }

            public void Update(GameTime gameTime)
            {
                for (var i = 0; i < _ringCounters.Length; i++)
                {
                    _ringCounters[i] += gameTime.ElapsedGameTime.TotalMilliseconds * _ringSpeeds[i];
                }

                _particleSystem.Update(gameTime);
            }
        }
    }
}