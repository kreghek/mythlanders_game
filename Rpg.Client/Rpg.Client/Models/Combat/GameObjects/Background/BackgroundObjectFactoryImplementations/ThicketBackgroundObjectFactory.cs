using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Models.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
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
            var list = new List<IBackgroundObject>();
            for (var i = 0; i <= 10; i++)
            {
                var y = (i * 71f) / 710;
                var cloud = new BackgroundCloud(
                    _gameObjectContentStorage.GetCombatBackgroundAnimatedObjectsTexture(CombatBackgroundObjectTextureType.Clouds),
                    i,
                    new Vector2(0, 40 + y),
                    new Vector2(1000, 40 + y),
                    1 + (i * 31f) / 310);

                list.Add(cloud);
            }

            return list;
        }

        public IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects()
        {
            var list = new List<IBackgroundObject>();

            var bannerObject = new PositionalAnimatedObject(
                _gameObjectContentStorage.GetCombatBackgroundAnimatedObjectsTexture(CombatBackgroundObjectTextureType.Banner),
                new Vector2(160, 480 - 128));
            list.Add(bannerObject);

            return list;
        }
    }
}