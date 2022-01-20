using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Party.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Party
{
    internal sealed class PartyScreen : GameScreenWithMenuBase
    {
        private const int CHARACTER_COUNT = 12;
        private const int PANEL_WIDTH = 128;
        private const int PANEL_HEIGHT = 128;
        private readonly IList<CharacterPanel> _characterPanels;
        private readonly GameObjectContentStorage _gameObjectsContentStorage;

        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _isInitialized;

        public PartyScreen(EwarGame game) : base(game)
        {
            _globeProvider = game.Services.GetService<GlobeProvider>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectsContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _characterPanels = new List<CharacterPanel>();
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

            DrawCharacters(spriteBatch: spriteBatch, contentRect: contentRect);

            DrawInventory(spriteBatch, contentRect);

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (!_isInitialized)
            {
                var characters = _globeProvider.Globe.Player.GetAll().OrderBy(x => x.UnitScheme.Name).ToArray();
                foreach (var character in characters)
                {
                    var resources = new CharacterPanelResources
                    (
                        buttonTexture: _uiContentStorage.GetButtonTexture(),
                        buttonFont: _uiContentStorage.GetTitlesFont(),
                        indicatorsTexture: _uiContentStorage.GetButtonIndicatorsTexture(),
                        portraitTexture: _gameObjectsContentStorage.GetUnitPortrains(),
                        nameFont: _uiContentStorage.GetTitlesFont(),
                        mainFont: _uiContentStorage.GetMainFont()
                    );

                    var panel = new CharacterPanel(texture: _uiContentStorage.GetPanelTexture(), character: character,
                        player: _globeProvider.Globe.Player, resources);
                    _characterPanels.Add(panel);

                    panel.SelectCharacter += Panel_SelectCharacter;
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

        private void DrawCharacters(SpriteBatch spriteBatch, Rectangle contentRect)
        {
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
        }

        private void DrawInventory(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int COL = 5;
            var array = _globeProvider.Globe.Player.Inventory.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                var resourceItem = array[i];

                var col = i % COL;
                var row = i / COL;

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    $"{resourceItem.Type} x {resourceItem.Amount}",
                    new Vector2(120 * col, (contentRect.Bottom - 60) + (row * 20)), Color.Wheat);
            }
        }

        private void Panel_SelectCharacter(object? sender, SelectCharacterEventArgs e)
        {
            var screenService = Game.Services.GetService<ScreenService>();
            screenService.Selected = e.Character;

            ScreenManager.ExecuteTransition(this, ScreenTransition.CharacterDetails);
        }
    }
}