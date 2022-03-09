using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Biome.GameObjects
{
    internal sealed class LocationGameObject
    {
        private const int CELL_SIZE = 256;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IList<SingleGameObject> _objects = new List<SingleGameObject>();
        private readonly Vector2 _position;
        private readonly Texture2D _texture;

        public LocationGameObject(int cellX, int cellY, Vector2 centerNodePosition, GlobeNodeSid nodeSid,
            GameObjectContentStorage gameObjectContentStorage, GlobeNode node)
        {
            // https://clintbellanger.net/articles/isometric_math/
            const int TILE_WIDTH_HALF = CELL_SIZE / 2;
            const int TILE_HEIGHT_HALF = CELL_SIZE / 2 / 2;

            var screenX = cellX * TILE_WIDTH_HALF - cellY * TILE_WIDTH_HALF;
            var screenY = cellX * TILE_HEIGHT_HALF + cellY * TILE_HEIGHT_HALF;

            var cellPosition = new Vector2(screenX, screenY);
            _position = cellPosition + centerNodePosition;
            _texture = gameObjectContentStorage.GetLocationTextures(nodeSid);
            _gameObjectContentStorage = gameObjectContentStorage;

            var graphicObjectPosition = new Vector2(128, 64) + _position;

            Configure(nodeSid, graphicObjectPosition);

            if (node.CombatSequence is not null)
            {
                NodeModel = new GlobeNodeGameObject(node, graphicObjectPosition - new Vector2(64, 0),
                    gameObjectContentStorage);
            }
        }

        internal GlobeNodeGameObject? NodeModel { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public IReadOnlyList<Sprite> GetLandscapeSprites()
        {
            return _objects.Where(x => x.IsLandscape).Select(x => x.GetSprite()).ToList();
        }

        public IReadOnlyList<Sprite> GetSprites()
        {
            return _objects.Where(x => !x.IsLandscape).Select(x => x.GetSprite()).ToList();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in _objects)
            {
                obj.Update(gameTime);
            }

            if (NodeModel is not null)
            {
                NodeModel.Update(gameTime);
            }
        }

        private void Configure(GlobeNodeSid nodeSid, Vector2 graphicObjectPosition)
        {
            switch (nodeSid)
            {
                case GlobeNodeSid.Thicket:

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition,
                        rowIndex: 0,
                        origin: new Vector2(0.5f, 0.5f),
                        _gameObjectContentStorage));

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-16, -16),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage));

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-8, -2),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage)
                    {
                        AnimationSpeedFactor = 1.1f
                    });

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(6, 4),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage)
                    {
                        AnimationSpeedFactor = 0.90f
                    });

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-12, 14),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage)
                    {
                        AnimationSpeedFactor = 1.3f
                    });

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-20, 24),
                        rowIndex: 2,
                        origin: new Vector2(0.5f, 0.5f),
                        _gameObjectContentStorage,
                        isLandscape: true));

                    break;

                case GlobeNodeSid.Battleground:

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(20, 20),
                        rowIndex: 3,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage));

                    break;

                case GlobeNodeSid.DestroyedVillage:

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-20, 24),
                        rowIndex: 5,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage));

                    break;

                case GlobeNodeSid.Swamp:

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-16, -16),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage));

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(6, 4),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage)
                    {
                        AnimationSpeedFactor = 0.90f
                    });

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition + new Vector2(-12, 14),
                        rowIndex: 1,
                        origin: new Vector2(0.5f, 1f),
                        _gameObjectContentStorage)
                    {
                        AnimationSpeedFactor = 1.3f
                    });

                    _objects.Add(new SingleGameObject(
                        graphicObjectPosition,
                        rowIndex: 4,
                        origin: new Vector2(0.5f, 0.5f),
                        _gameObjectContentStorage,
                        isLandscape: true));

                    break;
            }
        }
    }
}