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
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly SpriteFont _font;
        private readonly GameSettings _gameSettings;

        private readonly GlobeProvider _globeProvider;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly UnitName[] _showcaseUnits;
        private readonly SettingsModal _settingsModal;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public TitleScreen(EwarGame game)
            : base(game)
        {
            _globeProvider = Game.Services.GetService<GlobeProvider>();

            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _dice = Game.Services.GetService<IDice>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayTitleTrack();

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

            var buttonTexture = _uiContentStorage.GetButtonTexture();
            _font = _uiContentStorage.GetMainFont();

            _buttons = new List<ButtonBase>();

            var loadGameButton = CreateLoadButtonOrNothing(buttonTexture, _font);
            if (loadGameButton is not null)
            {
                _buttons.Add(loadGameButton);
            }
            else
            {
                var startButton = new ResourceTextButton(
                    nameof(UiResource.StartNewGameButtonTitle),
                    buttonTexture,
                    _font,
                    Rectangle.Empty);
                startButton.OnClick += StartButton_OnClick;

                _buttons.Add(startButton);
            }

            var settingsButton = new ResourceTextButton(
                nameof(UiResource.SettingsButtonTitle),
                buttonTexture,
                _font,
                Rectangle.Empty);
            settingsButton.OnClick += SettingsButton_OnClick;
            _buttons.Add(settingsButton);

            var creditsButton = new ResourceTextButton(
                nameof(UiResource.CreditsButtonTitle),
                buttonTexture,
                _font,
                Rectangle.Empty
            );
            creditsButton.OnClick += CreditsButton_OnClick;
            _buttons.Add(creditsButton);

            var exitGameButton = new ResourceTextButton(
                nameof(UiResource.ExitGameButtonTitle),
                buttonTexture,
                _font,
                Rectangle.Empty
            );
            exitGameButton.OnClick += (_, _) =>
            {
                game.Exit();
            };
            _buttons.Add(exitGameButton);

            _showcaseUnits = GetShowcaseHeroes();

            _settingsModal = new SettingsModal(_uiContentStorage, _resolutionIndependentRenderer, Game, this,
                isGameState: false);
            AddModal(_settingsModal, isLate: true);
        }

        private UnitName[] GetShowcaseHeroes()
        {
            var lastHeroes = GetLastHeroes(_globeProvider);
            return _dice.RollFromList(lastHeroes, 3).ToArray();
        }

        private static UnitName[] GetLastHeroes(GlobeProvider globeProvider)
        {
            var lastSave = globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).FirstOrDefault();

            if (lastSave is null)
            {
                return new[] { UnitName.Berimir };
            }
            else
            {
                var saveData = globeProvider.GetStoredData(lastSave.FileName);

                var activeUnits = saveData.Progress.Player.Group.Units.Select(x => x.SchemeSid);
                var poolUnits = saveData.Progress.Player.Pool.Units.Select(x => x.SchemeSid);

                var allUnits = activeUnits.Union(poolUnits);
                var unitNames = allUnits.Select(x => Enum.Parse<UnitName>(x)).ToArray();
                return unitNames;
            }
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

            var heroesRect = new Rectangle(0, 0, ResolutionIndependentRenderer.VirtualWidth, ResolutionIndependentRenderer.VirtualHeight / 2);
            DrawHeroes(spriteBatch, heroesRect);

            var menuRect = new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Center.Y, ResolutionIndependentRenderer.VirtualWidth, ResolutionIndependentRenderer.VirtualHeight / 2);
            DrawMenu(spriteBatch, menuRect);

            spriteBatch.End();
        }

        private void DrawHeroes(SpriteBatch spriteBatch, Rectangle heroesRect)
        {
            for (var i = 0; i < _showcaseUnits.Length; i++)
            {
                var heroSid = _showcaseUnits[i];

                var heroPosition = new Vector2(heroesRect.Width / _showcaseUnits.Length * i, heroesRect.Bottom - 64);
                spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(heroSid),
                    heroPosition,
                    new Rectangle(0, 0, 64, 64), Color.White);
            }
        }

        private void DrawMenu(SpriteBatch spriteBatch, Rectangle menuRect)
        {
            if (_gameSettings.Mode == GameMode.Demo)
            {
                spriteBatch.DrawString(_font, "Demo",
                    new Vector2(
                        menuRect.Center.X,
                        menuRect.Top + 10),
                    Color.White);
            }

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(
                    menuRect.X - BUTTON_WIDTH / 2,
                    menuRect.Top + 50 + index * 50,
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }

        private ButtonBase? CreateLoadButtonOrNothing(Texture2D buttonTexture, SpriteFont font)
        {
            if (!_globeProvider.CheckSavesExist())
            {
                return null;
            }

            if (Game.Services.GetService<GameSettings>().Mode == GameMode.Demo)
            {
                return null;
            }

            var loadGameButton = new ResourceTextButton(
                nameof(UiResource.ContinueGameButtonTitle),
                buttonTexture,
                font,
                new Rectangle(0, 0, 100, 25));

            loadGameButton.OnClick += (_, _) =>
            {
                var continueDialog = new ContinueGameModal(_uiContentStorage, _resolutionIndependentRenderer,
                    _globeProvider, _dice, _unitSchemeCatalog, _eventCatalog, ScreenManager, this);
                AddModal(continueDialog, isLate: true);
                continueDialog.Show();
            };

            return loadGameButton;
        }

        private void CreditsButton_OnClick(object? sender, EventArgs e)
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Credits);
        }

        private void SettingsButton_OnClick(object? sender, EventArgs e)
        {
            _settingsModal.Show();
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();
            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
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