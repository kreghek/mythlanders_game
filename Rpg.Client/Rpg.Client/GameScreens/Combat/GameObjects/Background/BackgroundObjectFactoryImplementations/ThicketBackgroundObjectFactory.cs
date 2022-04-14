using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class ThicketBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly BackgroundObjectCatalog _backgroundObjectCatalog;
        private readonly IDice _dice;

        public ThicketBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage, BackgroundObjectCatalog backgroundObjectCatalog, IDice dice)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
            _backgroundObjectCatalog = backgroundObjectCatalog;
            _dice = dice;
        }

        public IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects()
        {
            //throw new System.NotImplementedException();

            return new List<IBackgroundObject>(0);
        }

        public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
        {
            var list = new List<IBackgroundObject>();

            var weatherObject = new WeatherAnimatedObject(_gameObjectContentStorage.GetParticlesTexture(),
                new Rectangle(0, 32, 32, 32));
            list.Add(weatherObject);

            return list;
        }

        public void GenerateFarLayerObjects(SpriteContainer spriteContainer)
        {
            var availableObjects = _backgroundObjectCatalog.GetObjects();

            for (var i = 0; i < 4 * 4; i++)
            {
                if (_dice.RollD100() > 25)
                {
                    var rolledObjectInfo = _dice.RollFromList(availableObjects.ToArray());

                    var sprite = new Sprite(rolledObjectInfo.SpriteAtlas);
                    sprite.SourceRectangle = rolledObjectInfo.SourceRectangle;
                    sprite.Position = new Vector2(i * 64, 256);
                    sprite.Origin = new Vector2(0.5f, 1);
                    spriteContainer.AddChild(sprite);
                }
            }
        }
    }
}