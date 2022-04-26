using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class ThicketBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public ThicketBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
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
    }
}