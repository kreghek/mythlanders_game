using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Upgrade.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Upgrade
{
    internal sealed class UpgradeScreen : GameScreenWithMenuBase
    {
        private const int CHARACTER_COUNT = 12;
        private const int PANEL_WIDTH = 128;
        private const int PANEL_HEIGHT = 128;
        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectsContentStorage;
        private bool _isInitialized;
        private readonly IList<CharacterPanel> _characterPanels;

        public UpgradeScreen(EwarGame game) : base(game)
        {
            _globeProvider = game.Services.GetService<GlobeProvider>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectsContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _characterPanels = new List<CharacterPanel>();
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            if (!_isInitialized)
            {
                return;
            }

            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            for (var i = 0; i < _characterPanels.Count; i++)
            {
                var panel = _characterPanels[i];
                var col = i % CHARACTER_COUNT;
                var row = i / CHARACTER_COUNT;
                var panelOffset = new Point(col * (PANEL_WIDTH + 5), row * (PANEL_HEIGHT + 5));
                
                panel.Rect =
                    new Rectangle(
                        contentRect.Location + panelOffset,
                        new Point(PANEL_WIDTH, PANEL_HEIGHT));
                panel.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);
            
            if (!_isInitialized)
            {
                foreach (var character in _globeProvider.Globe.Player.GetAll())
                {
                    var panel = new CharacterPanel(
                        texture: _uiContentStorage.GetPanelTexture(),
                        character: character,
                        buttonTexture: _uiContentStorage.GetButtonTexture(),
                        buttonFont: _uiContentStorage.GetTitlesFont(),
                        portraitTexture: _gameObjectsContentStorage.GetUnitPortrains(),
                        nameFont: _uiContentStorage.GetTitlesFont(),
                        mainFont: _uiContentStorage.GetMainFont());
                    _characterPanels.Add(panel);
                }

                _isInitialized = true;
            }
            else
            {
                foreach (var characterPanel in _characterPanels)
                {
                    characterPanel.Update(ResolutionIndependentRenderer);
                }
            }
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle),
                _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont());
            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
            };

            return new ButtonBase[] { backButton };
        }
    }
}
