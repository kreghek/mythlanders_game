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
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly GameSettings _gameSettings;

        private readonly GlobeProvider _globeProvider;

        private readonly ParticleSystem _particleSystem;
        private readonly ParticleSystem[] _pulseParticleSystems;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly SettingsModal _settingsModal;
        private readonly UnitName[] _showcaseUnits;
        private readonly IUiContentStorage _uiContentStorage;
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
            var buttonFont = _uiContentStorage.GetMainFont();

            _buttons = new List<ButtonBase>();

            var loadGameButton = CreateLoadButtonOrNothing(buttonTexture, buttonFont);
            if (loadGameButton is not null)
            {
                _buttons.Add(loadGameButton);
            }
            else
            {
                var startButton = new ResourceTextButton(
                    nameof(UiResource.PlayGameButtonTitle),
                    buttonTexture,
                    buttonFont,
                    Rectangle.Empty);
                startButton.OnClick += StartButton_OnClick;

                _buttons.Add(startButton);
            }

            var settingsButton = new ResourceTextButton(
                nameof(UiResource.SettingsButtonTitle),
                buttonTexture,
                buttonFont,
                Rectangle.Empty);
            settingsButton.OnClick += SettingsButton_OnClick;
            _buttons.Add(settingsButton);

            var creditsButton = new ResourceTextButton(
                nameof(UiResource.CreditsButtonTitle),
                buttonTexture,
                buttonFont,
                Rectangle.Empty
            );
            creditsButton.OnClick += CreditsButton_OnClick;
            _buttons.Add(creditsButton);

            var exitGameButton = new ResourceTextButton(
                nameof(UiResource.ExitGameButtonTitle),
                buttonTexture,
                buttonFont,
                Rectangle.Empty
            );
            exitGameButton.OnClick += (_, _) =>
            {
                game.Exit();
            };
            _buttons.Add(exitGameButton);

            _showcaseUnits = GetShowcaseHeroes();

            var generator =
                new HorizontalPulseParticleGenerator2(new[] { _gameObjectContentStorage.GetParticlesTexture() });
            _particleSystem =
                new ParticleSystem(_resolutionIndependentRenderer.VirtualBounds.Center.ToVector2(), generator);

            _pulseParticleSystems = new[]
            {
                CreatePulseParticleSystem(_resolutionIndependentRenderer.VirtualBounds.Center.ToVector2() +
                                          new Vector2(0, 80)),
                CreatePulseParticleSystem(_resolutionIndependentRenderer.VirtualBounds.Center.ToVector2()
                                          + Vector2.UnitX * (0.25f * _resolutionIndependentRenderer.VirtualWidth) +
                                          new Vector2(0, 60)),
                CreatePulseParticleSystem(_resolutionIndependentRenderer.VirtualBounds.Center.ToVector2()
                    - Vector2.UnitX * (0.25f * _resolutionIndependentRenderer.VirtualWidth) + new Vector2(0, 50))
            };

            _settingsModal = new SettingsModal(_uiContentStorage, _resolutionIndependentRenderer, Game, this,
                isGameState: false);
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

            var heroesRect = new Rectangle(0, 0, ResolutionIndependentRenderer.VirtualWidth,
                ResolutionIndependentRenderer.VirtualHeight / 2);
            DrawHeroes(spriteBatch, heroesRect);

            var logoRect = new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Center.Y - 128,
                ResolutionIndependentRenderer.VirtualWidth, 64);
            DrawLogo(spriteBatch, logoRect);

            var menuRect = new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Center.Y,
                ResolutionIndependentRenderer.VirtualWidth, ResolutionIndependentRenderer.VirtualHeight / 2);
            DrawMenu(spriteBatch, menuRect);

            if (_gameSettings.Mode == GameMode.Demo)
            {
                spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), "Demo",
                    new Vector2(
                        ResolutionIndependentRenderer.VirtualBounds.Right - 100,
                        ResolutionIndependentRenderer.VirtualBounds.Top + 10),
                    Color.White);
            }

            var socialPosition = new Vector2(ResolutionIndependentRenderer.VirtualBounds.Right - 75,
                ResolutionIndependentRenderer.VirtualBounds.Bottom - 150);
            spriteBatch.Draw(_uiContentStorage.GetSocialTexture(), socialPosition, Color.White);

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependentRenderer);
            }

            _particleSystem.Update(gameTime);
            foreach (var particleSystem in _pulseParticleSystems)
            {
                particleSystem.Update(gameTime);
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
                nameof(UiResource.PlayGameButtonTitle),
                buttonTexture,
                font,
                new Rectangle(0, 0, 100, 25));

            loadGameButton.OnClick += (_, _) =>
            {
                var continueDialog = new ContinueGameModal(_uiContentStorage, _resolutionIndependentRenderer,
                    _globeProvider, _dice, _eventCatalog, ScreenManager, this);
                AddModal(continueDialog, isLate: true);
                continueDialog.Show();
            };

            return loadGameButton;
        }

        private ParticleSystem CreatePulseParticleSystem(Vector2 position)
        {
            var generator =
                new HorizontalPulseParticleGenerator(new[] { _gameObjectContentStorage.GetParticlesTexture() });
            var particleSystem = new ParticleSystem(position, generator);

            return particleSystem;
        }

        private void CreditsButton_OnClick(object? sender, EventArgs e)
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Credits);
        }

        private void DrawHeroes(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var offsets = new[] { Vector2.Zero, new Vector2(1, -1), new Vector2(-1, -1) };
            for (var i = _showcaseUnits.Length - 1; i >= 0; i--)
            {
                var heroSid = _showcaseUnits[i];

                var heroPosition = new Vector2(contentRect.Center.X - 256 / 2, contentRect.Bottom - 256) +
                                   new Vector2(128 * offsets[i].X, 24 * offsets[i].Y);
                spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(heroSid),
                    heroPosition,
                    new Rectangle(0, 0, 256, 256), Color.White);
            }
        }

        private void DrawLogo(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            foreach (var particleSystem in _pulseParticleSystems)
            {
                particleSystem.Draw(spriteBatch);
            }

            _particleSystem.MoveEmitter(contentRect.Center.ToVector2() + new Vector2(0, 160));

            _particleSystem.Draw(spriteBatch);

            spriteBatch.Draw(_uiContentStorage.GetLogoTexture(),
                new Vector2(contentRect.Center.X - _uiContentStorage.GetLogoTexture().Width / 2, contentRect.Top),
                Color.White);
        }

        private void DrawMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(
                    contentRect.Center.X - BUTTON_WIDTH / 2,
                    contentRect.Top + 50 + index * (BUTTON_HEIGHT + 5),
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }
        }

        private static UnitName[] GetLastHeroes(GlobeProvider globeProvider)
        {
            var lastSave = globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).FirstOrDefault();

            if (lastSave is null)
            {
                return new[] { UnitName.Berimir };
            }

            var saveData = globeProvider.GetStoredData(lastSave.FileName);

            var activeUnits = saveData.Progress.Player.Group.Units.Select(x => x.SchemeSid);
            var poolUnits = saveData.Progress.Player.Pool.Units.Select(x => x.SchemeSid);

            var allUnits = activeUnits.Union(poolUnits);
            var unitNames = allUnits.Select(x => Enum.Parse<UnitName>(x)).ToArray();
            return unitNames;
        }

        private UnitName[] GetShowcaseHeroes()
        {
            if (_gameSettings.Mode == GameMode.Demo)
            {
                return new[] { UnitName.Berimir, UnitName.Hawk, UnitName.Rada };
            }

            var lastHeroes = GetLastHeroes(_globeProvider);

            if (lastHeroes.Count() > 3)
            {
                return _dice.RollFromList(lastHeroes, 3).ToArray();
            }

            return lastHeroes;
        }

        private void SettingsButton_OnClick(object? sender, EventArgs e)
        {
            _settingsModal.Show();
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();

            _globeProvider.Globe.IsNodeInitialized = true;

            var firstAvailableNodeInBiome =
                _globeProvider.Globe.Biomes.SelectMany(x => x.Nodes).SingleOrDefault(x => x.IsAvailable);

            _globeProvider.Globe.ActiveCombat = new Core.Combat(_globeProvider.Globe.Player.Party,
                firstAvailableNodeInBiome,
                firstAvailableNodeInBiome.CombatSequence.Combats.First(), _dice,
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