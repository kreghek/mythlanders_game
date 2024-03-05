using System.Collections.Generic;
using System.Linq;

using Client.Assets;

using CombatDicesTeam.Dices;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations;

internal sealed class MonasteryBackgroundObjectFactory : IBackgroundObjectFactory
{
    private readonly IDice _dice;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public MonasteryBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage, IDice dice)
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
                    _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
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
        //var list = new List<IBackgroundObject>();

        //for (var i = 0; i < 100; i++)
        //{
        //    var weatherObject = new DemonicFireAnimatedObject(_gameObjectContentStorage.GetParticlesTexture(),
        //        new Rectangle(0, 32, 32, 32),
        //        new Vector2(i * 10, 240));
        //    list.Add(weatherObject);
        //}

        //return list;

        return new List<IBackgroundObject>(0);
    }

    public IReadOnlyList<IBackgroundObject> CreateMainLayerObjects()
    {
        var mainLayerObjects = new List<IBackgroundObject>();

        const int FLOOR_COUNT = 1024 / 256 + 1;

        for (var objectIndex = 0; objectIndex < FLOOR_COUNT; objectIndex++)
        {
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
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(LocationTheme.ChineseMonastery,
                    BackgroundLayerType.Main, 0),
                position,
                sourceRectangle,
                new Vector2(0, 0));

            mainLayerObjects.Add(floorObject);
        }

        return mainLayerObjects;
    }

    public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
    {
        return new List<IBackgroundObject>(0);
    }

    public IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
    {
        return new List<IBackgroundObject>(0); //CreateHouses(farLayerBottom: 180);
    }
}