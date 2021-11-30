using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Map
{
    internal class MapScreen : GameScreenBase
    {
        private readonly IList<TextButton> _biomButtons;
        private readonly Camera2D _camera;
        private readonly Globe _globe;

        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _isNodeModelsCreated;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public MapScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();

            soundtrackManager.PlayMapTrack();

            var globe = game.Services.GetService<GlobeProvider>().Globe;
            _globe = globe;

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _biomButtons = new List<TextButton>();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

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
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(Game.Services.GetService<IDice>(), _unitSchemeCatalog, Game.Services.GetService<IEventCatalog>());
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
                        button.Update(_resolutionIndependentRenderer);
                    }
                }
            }
        }
    }
}