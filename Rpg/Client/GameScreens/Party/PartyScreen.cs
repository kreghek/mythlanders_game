using System.Collections.Generic;
using System.Linq;
using Client;
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
        private const int MARGIN = 5;
        private const int COLUMN_COUNT = 5;
        private const int PANEL_WIDTH = 128;
        private const int PANEL_HEIGHT = 128;
        private readonly IList<HeroPanel> _characterPanels;
        private readonly GameObjectContentStorage _gameObjectsContentStorage;

        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;

        public PartyScreen(EwarGame game) : base(game)
        {
            _globeProvider = game.Services.GetService<GlobeProvider>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectsContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _characterPanels = new List<HeroPanel>();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));
            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map, null);
            };

            return new ButtonBase[] { backButton };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
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

        protected override void InitializeContent()
        {
            var heroes = _globeProvider.Globe.Player.GetAll().OrderBy(x => x.UnitScheme.Name).ToArray();
            foreach (var hero in heroes)
            {
                var resources = new HeroPanelResources
                (
                    buttonTexture: _uiContentStorage.GetControlBackgroundTexture(),
                    buttonFont: _uiContentStorage.GetTitlesFont(),
                    indicatorsTexture: _uiContentStorage.GetButtonIndicatorsTexture(),
                    portraitTexture: _gameObjectsContentStorage.GetUnitPortrains(),
                    disabledTexture: _uiContentStorage.GetDisabledTexture(),
                    nameFont: _uiContentStorage.GetTitlesFont(),
                    mainFont: _uiContentStorage.GetMainFont()
                );

                var panel = new HeroPanel(hero, _globeProvider.Globe.Player, resources);
                _characterPanels.Add(panel);

                panel.SelectCharacter += Panel_SelectCharacter;
            }
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var characterPanel in _characterPanels)
            {
                characterPanel.Update(ResolutionIndependentRenderer);
            }
        }

        private void DrawCharacters(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            for (var i = 0; i < _characterPanels.Count; i++)
            {
                var panel = _characterPanels[i];
                var col = i % COLUMN_COUNT;
                var row = i / COLUMN_COUNT;
                var panelOffset = new Point(col * (PANEL_WIDTH + MARGIN), row * (PANEL_HEIGHT + MARGIN));

                panel.Rect =
                    new Rectangle(
                        contentRect.Location + panelOffset,
                        new Point(PANEL_WIDTH, PANEL_HEIGHT));
                panel.Draw(spriteBatch);
            }
        }

        private void DrawInventory(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int RESOURCE_PANEL_X_POS = COLUMN_COUNT * (PANEL_WIDTH + MARGIN);
            const int TEXT_HEIGHT = 20;

            var resources = _globeProvider.Globe.Player.Inventory
                .Where(x => x.Amount > 0)
                .ToArray();

            for (var i = 0; i < resources.Length; i++)
            {
                var resourceItem = resources[i];

                var resourceName = GameObjectHelper.GetLocalized(resourceItem.Type);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    $"{resourceName} x {resourceItem.Amount}",
                    new Vector2(contentRect.Left + RESOURCE_PANEL_X_POS, i * TEXT_HEIGHT), Color.Wheat);
            }
        }

        private void Panel_SelectCharacter(object? sender, SelectHeroEventArgs e)
        {
            var screenService = Game.Services.GetService<ScreenService>();
            screenService.Selected = e.Character;

            // TODO Pass selected hero via args instead screenService
            ScreenManager.ExecuteTransition(this, ScreenTransition.Hero, null);
        }
    }
}