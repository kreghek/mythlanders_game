﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client;
using Client.GameScreens.CommandCenter;

using Core.Dices;

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
        private const int TITLE_PORTRAIT_COUNT = 3;
        private readonly IList<ButtonBase> _buttons;
        private readonly Camera2D _camera;
        private readonly ICampaignGenerator _campaignGenerator;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly GameSettings _gameSettings;

        private readonly GlobeProvider _globeProvider;

        private readonly ParticleSystem _particleSystem;
        private readonly ParticleSystem[] _pulseParticleSystems;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;

        private readonly Random _rnd = new Random();
        private readonly SettingsModal _settingsModal;
        private readonly UnitName[] _showcaseUnits;
        private readonly IUiContentStorage _uiContentStorage;
        private Vector2 _bgCurrentPosition;

        private Vector2 _bgMoveVector = Vector2.One * 0.2f;

        public TitleScreen(EwarGame game)
            : base(game)
        {
            _globeProvider = Game.Services.GetService<GlobeProvider>();

            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();
            _campaignGenerator = game.Services.GetService<ICampaignGenerator>();

            _dice = Game.Services.GetService<IDice>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayTitleTrack();

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

            var buttonTexture = _uiContentStorage.GetControlBackgroundTexture();
            var buttonFont = _uiContentStorage.GetMainFont();

            _buttons = new List<ButtonBase>();

            var loadGameButton = CreateLoadButtonOrNothing();
            if (loadGameButton is not null)
            {
                _buttons.Add(loadGameButton);
            }
            else
            {
                var startButton = new ResourceTextButton(nameof(UiResource.PlayGameButtonTitle));
                startButton.OnClick += StartButton_OnClick;

                _buttons.Add(startButton);
            }

            var settingsButton = new ResourceTextButton(nameof(UiResource.SettingsButtonTitle));
            settingsButton.OnClick += SettingsButton_OnClick;
            _buttons.Add(settingsButton);

            var creditsButton = new ResourceTextButton(nameof(UiResource.CreditsButtonTitle));
            creditsButton.OnClick += CreditsButton_OnClick;
            _buttons.Add(creditsButton);

            var exitGameButton = new ResourceTextButton(nameof(UiResource.ExitGameButtonTitle));
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
                isGameStarted: false);
            AddModal(_settingsModal, isLate: true);
        }

        public void StartClearNewGame(GlobeProvider globeProvider, IEventCatalog eventCatalog,
            IScreen currentScreen, IScreenManager screenManager,
            Action? clearScreenHandlersDelegate)
        {
            globeProvider.GenerateNew();

            var campaigns = _campaignGenerator.CreateSet();

            screenManager.ExecuteTransition(
                currentScreen,
                ScreenTransition.CampaignSelection,
                new CommandCenterScreenTransitionArguments
                {
                    AvailableCampaigns = campaigns
                });

            //globeProvider.Globe.IsNodeInitialized = true;

            //var firstAvailableNodeInBiome =
            //    globeProvider.Globe.Biomes.SelectMany(x => x.Nodes)
            //        .First(x => x.IsAvailable);

            //MapScreen.HandleLocationSelect(autoCombat: false, node: firstAvailableNodeInBiome,
            //    availableEvent: firstAvailableNodeInBiome.AssignedEvent,
            //    eventCatalog: eventCatalog,
            //    currentScreen: currentScreen,
            //    screenManager,
            //    clearScreenHandlersDelegate);
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

            spriteBatch.Draw(_uiContentStorage.GetTitleBackgroundTexture(), _bgCurrentPosition, Color.White);
            spriteBatch.Draw(_uiContentStorage.GetModalShadowTexture(),
                new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Center.X - 128, 0, 256, 480),
                sourceRectangle: null, Color.Lerp(Color.White, Color.Transparent, 0.3f));

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

        protected override void InitializeContent()
        {
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

            void CreateRandomMovement()
            {
                _bgMoveVector = (new Vector2((float)_rnd.NextDouble(), (float)_rnd.NextDouble()) - Vector2.One * 0.5f) *
                                0.1f;
            }

            if (_bgCurrentPosition.X < -150)
            {
                _bgCurrentPosition.X = -150;
                CreateRandomMovement();
            }
            else if (_bgCurrentPosition.X > 0)
            {
                _bgCurrentPosition.X = 0;
                CreateRandomMovement();
            }
            else if (_bgCurrentPosition.Y < -20)
            {
                _bgCurrentPosition.Y = -20;
                CreateRandomMovement();
            }
            else if (_bgCurrentPosition.Y > 0)
            {
                _bgCurrentPosition.Y = 0;
                CreateRandomMovement();
            }
            else
            {
                _bgCurrentPosition += _bgMoveVector;
            }
        }

        private ButtonBase? CreateLoadButtonOrNothing()
        {
            if (!_globeProvider.CheckSavesExist())
            {
                return null;
            }

            if (Game.Services.GetService<GameSettings>().Mode == GameMode.Demo)
            {
                return null;
            }

            var loadGameButton = new ResourceTextButton(nameof(UiResource.PlayGameButtonTitle));

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
            ScreenManager.ExecuteTransition(this, ScreenTransition.Credits, null);
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

        private UnitName[] GetAvailableHeroes()
        {
            if (_gameSettings.Mode == GameMode.Demo)
            {
                return new[]
                {
                    UnitName.Swordsman, UnitName.Archer, UnitName.Herbalist, UnitName.Assaulter, UnitName.Monk,
                    UnitName.Spearman, UnitName.Hoplite
                };
            }

            var lastHeroes = GetLastHeroes(_globeProvider);

            return lastHeroes;
        }

        private static UnitName[] GetLastHeroes(GlobeProvider globeProvider)
        {
            var lastSave = globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).FirstOrDefault();

            if (lastSave is null)
            {
                return new[] { UnitName.Swordsman };
            }

            var saveData = globeProvider.GetStoredData(lastSave.FileName);

            var activeUnits = saveData.Progress.Player.Group.Units.Select(x => x.SchemeSid);
            var poolUnits = saveData.Progress.Player.Pool.Units.Select(x => x.SchemeSid);

            var allUnits = activeUnits.Union(poolUnits);
            var unitNames = allUnits.Select(x => GetLastHeroName(x)).ToArray();
            return unitNames;
        }

        private static UnitName GetLastHeroName(string storedSid)
        {
            if (Enum.TryParse<UnitName>(storedSid, out var sid))
            {
                return sid;
            }

            return UnitName.Undefined;
        }

        private UnitName[] GetShowcaseHeroes()
        {
            var lastHeroes = GetAvailableHeroes();

            var heroCount = Math.Min(TITLE_PORTRAIT_COUNT, lastHeroes.Length);

            return _dice.RollFromList(lastHeroes, heroCount).ToArray();
        }

        private void SettingsButton_OnClick(object? sender, EventArgs e)
        {
            _settingsModal.Show();
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            StartClearNewGame(_globeProvider, _eventCatalog, this, ScreenManager, () => { });
        }
    }
}