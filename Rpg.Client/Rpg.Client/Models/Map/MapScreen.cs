using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biom;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Map
{
    internal class MapScreen : GameScreenBase
    {
        private readonly Globe _globe;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _isNodeModelsCreated;

        private readonly IList<TextButton> _biomButtons;

        public MapScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _globe = globe;

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _biomButtons = new List<TextButton>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(Game.Services.GetService<IDice>());
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    foreach (var biom in _globe.Bioms.Where(x => x.IsAvailable).ToArray())
                    {
                        var button = new TextButton(biom.Name, _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), Rectangle.Empty);
                        button.OnClick += (s, e) =>
                        {
                            _globe.CurrentBiom = biom;

                            TargetScreen = new BiomScreen(Game, SpriteBatch);
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
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();

            if (_isNodeModelsCreated)
            {
                var index = 0;
                foreach (var button in _biomButtons)
                {
                    button.Rect = new Rectangle(100, 100 + index * 30, 200, 25);
                    button.Draw(SpriteBatch);
                    index++;
                }
            }

            SpriteBatch.End();
        }
    }
}