using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Map
{
    internal class MapScreen : GameScreenBase
    {
        private readonly IList<TextButton> _biomButtons;
        private readonly Globe _globe;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _isNodeModelsCreated;

        public MapScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globe = game.Services.GetService<GlobeProvider>().Globe;
            _globe = globe;

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _biomButtons = new List<TextButton>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (_isNodeModelsCreated)
            {
                var index = 0;
                foreach (var button in _biomButtons)
                {
                    button.Rect = new Rectangle(100, 100 + index * 30, 200, 25);
                    button.Draw(spriteBatch);
                    index++;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(Game.Services.GetService<IDice>());
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    foreach (var biom in _globe.Biomes.Where(x => x.IsAvailable).ToArray())
                    {
                        var button = new TextButton(biom.Type.ToString(), _uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetMainFont(), Rectangle.Empty);
                        button.OnClick += (s, e) =>
                        {
                            _globe.CurrentBiome = biom;

                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                        };
                        _biomButtons.Add(button);
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    foreach (var button in _biomButtons)
                    {
                        button.Update();
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}