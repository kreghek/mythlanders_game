using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Title
{
    internal sealed class TitleScreen : GameScreenBase
    {
        private const int BUTTON_HEIGHT = 20;

        private const int BUTTON_WIDTH = 100;

        private readonly IList<ButtonBase> _buttons;
        private readonly Camera2D _camera;

        private readonly GlobeProvider _globeProvider;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly SettingsModal _settingsModal;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private readonly IDice _dice;
        private readonly GameSettings _gameSettings;
        private readonly SpriteFont _font;

        public TitleScreen(EwarGame game)
            : base(game)
        {
            _globeProvider = Game.Services.GetService<GlobeProvider>();

            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _dice = Game.Services.GetService<IDice>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayTitleTrack();

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            var buttonTexture = _uiContentStorage.GetButtonTexture();
            _font = _uiContentStorage.GetMainFont();

            _buttons = new List<ButtonBase>();

            var emptyRect = new Rectangle();
            var startButton = new TextButton(
                UiResource.StartGameButtonTitle,
                buttonTexture,
                _font,
                emptyRect);
            startButton.OnClick += StartButton_OnClick;
            _buttons.Add(startButton);

            var settingsButton = new TextButton(
                UiResource.SettingsButtonTitle,
                buttonTexture,
                _font,
                emptyRect);
            settingsButton.OnClick += SettingsButton_OnClick;
            _buttons.Add(settingsButton);

            if (_gameSettings.Mode == GameMode.Full)
            {
                var loadGameButton = GetLoadButton(buttonTexture, _font);
                if (loadGameButton != null)
                {
                    _buttons.Add(loadGameButton);
                }
            }

            _settingsModal = new SettingsModal(_uiContentStorage, _resolutionIndependentRenderer, Game, this);
            AddModal(_settingsModal, isLate: true);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            if (_gameSettings.Mode == GameMode.Demo)
            {
                spriteBatch.DrawString(_font, "Demo",
                    new Vector2(
                        _resolutionIndependentRenderer.VirtualBounds.Center.X,
                        110),
                    Color.White);
            }

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(
                    _resolutionIndependentRenderer.VirtualBounds.Center.X - BUTTON_WIDTH / 2,
                    150 + index * 50,
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }

        private ButtonBase? GetLoadButton(Texture2D buttonTexture, SpriteFont font)
        {
            if (!_globeProvider.CheckExistsSave())
            {
                return null;
            }

            if (Game.Services.GetService<GameSettings>().Mode == GameMode.Demo)
            {
                return null;
            }

            var loadGameButton = new TextButton(
                UiResource.LoadLastSaveButtonTitle,
                buttonTexture,
                font,
                new Rectangle(0, 0, 100, 25));

            loadGameButton.OnClick += (s, e) =>
            {
                var isSuccessLoaded = _globeProvider.LoadGlobe();
                if (!isSuccessLoaded)
                {
                    return;
                }

                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };

            return loadGameButton;
        }

        private void SettingsButton_OnClick(object? sender, EventArgs e)
        {
            _settingsModal.Show();
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();
            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog);
            _globeProvider.Globe.IsNodeInitialied = true;

            var biomes = _globeProvider.Globe.Biomes.Where(x => x.IsAvailable).ToArray();

            var startBiome = biomes.First();

            _globeProvider.Globe.CurrentBiome = startBiome;

            var firstAvailableNodeInBiome = startBiome.Nodes.SingleOrDefault(x => x.IsAvailable);

            _globeProvider.Globe.ActiveCombat = new Core.Combat(_globeProvider.Globe.Player.Party,
                firstAvailableNodeInBiome,
                firstAvailableNodeInBiome.CombatSequence.Combats.First(), startBiome,
                _dice,
                isAutoplay: false);

            if (firstAvailableNodeInBiome?.AssignedEvent is not null)
            {
                // Make same operations as on click on the first node on the biome screen. 
                _globeProvider.Globe.CurrentEvent = firstAvailableNodeInBiome.AssignedEvent;
                _globeProvider.Globe.CurrentEventNode = _globeProvider.Globe.CurrentEvent.BeforeCombatStartNode;

                _globeProvider.Globe.CurrentEvent.Counter++;

                ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
            }
            else
            {
                // Defensive case

                ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
            }
        }
    }
}