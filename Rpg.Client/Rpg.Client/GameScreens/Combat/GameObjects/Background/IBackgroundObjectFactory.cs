using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects.Background
{
    internal interface IBackgroundObjectFactory
    {
        IReadOnlyList<IBackgroundObject> CreateCloudLayerObjects();

        IReadOnlyList<IBackgroundObject> CreateFarLayerObjects()
        {
            return new List<IBackgroundObject>(0);
        }

        IReadOnlyList<IBackgroundObject> CreateForegroundLayerObjects();

        void GenerateFarLayerObjects(SpriteContainer layer) { }
    }

    internal sealed class BackgroundObjectCatalog
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public BackgroundObjectCatalog(GameObjectContentStorage gameObjectContentStorage)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public IReadOnlyCollection<BackgroundObjectBase> GetObjects()
        {
            return new BackgroundObjectBase[] {
                new StaticBackgroundObject(_gameObjectContentStorage.GetCombatBackgroundObjectsTexture(), new Rectangle(256 * 0, 0, 256, 256)),
                new StaticBackgroundObject(_gameObjectContentStorage.GetCombatBackgroundObjectsTexture(), new Rectangle(256 * 1, 0, 256, 256)),
                new StaticBackgroundObject(_gameObjectContentStorage.GetCombatBackgroundObjectsTexture(), new Rectangle(256 * 0, 256 * 1, 256, 256)),
                new StaticBackgroundObject(_gameObjectContentStorage.GetCombatBackgroundObjectsTexture(), new Rectangle(256 * 1, 256 * 1, 256, 256))
            };
        }
    }

    internal abstract class BackgroundObjectBase
    {
        public int Size { get; }
        public Texture2D SpriteAtlas { get; }
        public Rectangle SourceRectangle { get; }

        protected BackgroundObjectBase(Texture2D spriteAtlas, Rectangle sourceRectangle)
        {
            Size = 4;
            SpriteAtlas = spriteAtlas;
            SourceRectangle = sourceRectangle;
        }
    }

    internal sealed class StaticBackgroundObject : BackgroundObjectBase
    {
        public StaticBackgroundObject(Texture2D spriteAtlas, Rectangle sourceRectangle) : base(spriteAtlas, sourceRectangle)
        {
        }
    }
}