using System.Collections.Generic;

using Client.GameScreens;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background.BackgroundObjectFactoryImplementations
{
    internal sealed class BattlegroundBackgroundObjectFactory : IBackgroundObjectFactory
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public BattlegroundBackgroundObjectFactory(GameObjectContentStorage gameObjectContentStorage)
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
                    _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(BackgroundType.SlavicBattleground,
                        BackgroundLayerType.Clouds, 0),
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
                _gameObjectContentStorage.GetCombatBackgroundObjectsTexture(
                    BackgroundType.SlavicBattleground,
                    BackgroundLayerType.Closest,
                    0),
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 0, 1, 2, 3 }, fps: 2, isLoop: true,
                    frameWidth: 256, frameHeight: 256, textureColumns: 1),
                new Vector2(160, 480 - 128));
            list.Add(bannerObject);

            return list;
        }
    }
}