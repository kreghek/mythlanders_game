using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Assets.StageItems;
using Client.Core;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Combat;
using Client.GameScreens.CommandCenter;
using Client.GameScreens.Common;
using Client.ScreenManagement;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;

using Core.PropDrop;

using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Title;

internal sealed class TitleScreen : GameScreenBase
{
    private const int BUTTON_HEIGHT = 30;

    private const int BUTTON_WIDTH = 150;
    private const int TITLE_PORTRAIT_COUNT = 3;

    private readonly PongRectangleControl _bgPong;
    private readonly IList<ButtonBase> _buttons;
    private readonly ICamera2DAdapter _camera;
    private readonly ICampaignGenerator _campaignGenerator;
    private readonly IDice _dice;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly GameSettings _gameSettings;

    private readonly GlobeProvider _globeProvider;

    private readonly ParticleSystem _particleSystem;
    private readonly ParticleSystem[] _pulseParticleSystems;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;

    private readonly SettingsModal _settingsModal;
    private readonly UnitName[] _showcaseUnits;
    private readonly IUiContentStorage _uiContentStorage;

    public TitleScreen(TestamentGame game)
        : base(game)
    {
        _globeProvider = Game.Services.GetService<GlobeProvider>();

        _camera = Game.Services.GetService<ICamera2DAdapter>();
        _resolutionIndependentRenderer = Game.Services.GetService<IResolutionIndependentRenderer>();
        _campaignGenerator = game.Services.GetService<ICampaignGenerator>();

        _dice = Game.Services.GetService<IDice>();
        _gameSettings = Game.Services.GetService<GameSettings>();

        var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
        soundtrackManager.PlayTitleTrack();

        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

        _buttons = new List<ButtonBase>();

        var loadGameButton = CreateLoadButtonOrNothing();
        if (loadGameButton is not null)
        {
            _buttons.Add(loadGameButton);
        }
        else
        {
            var startButton = new TitleResourceTextButton(nameof(UiResource.PlayStoryButtonTitle));
            startButton.OnClick += StartButton_OnClick;

            _buttons.Add(startButton);
        }

        var freeCombatButton = new TitleResourceTextButton(nameof(UiResource.PlayFreeCombatButtonTitle));
        freeCombatButton.OnClick += FreeCombatButton_OnClick;
        _buttons.Add(freeCombatButton);

        var settingsButton = new TitleResourceTextButton(nameof(UiResource.SettingsButtonTitle));
        settingsButton.OnClick += SettingsButton_OnClick;
        _buttons.Add(settingsButton);

        var creditsButton = new TitleResourceTextButton(nameof(UiResource.CreditsButtonTitle));
        creditsButton.OnClick += CreditsButton_OnClick;
        _buttons.Add(creditsButton);

        var exitGameButton = new TitleResourceTextButton(nameof(UiResource.ExitGameButtonTitle));
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
                                      + Vector2.UnitX * (0.25f * _resolutionIndependentRenderer.VirtualBounds.Width) +
                                      new Vector2(0, 60)),
            CreatePulseParticleSystem(_resolutionIndependentRenderer.VirtualBounds.Center.ToVector2()
                - Vector2.UnitX * (0.25f * _resolutionIndependentRenderer.VirtualBounds.Width) + new Vector2(0, 50))
        };

        _settingsModal = new SettingsModal(_uiContentStorage, _resolutionIndependentRenderer, Game, this,
            isGameStarted: false);
        AddModal(_settingsModal, isLate: true);

        var bgTexture = _uiContentStorage.GetTitleBackgroundTexture();
        _bgPong = new PongRectangleControl(new Point(bgTexture.Width, bgTexture.Height),
            ResolutionIndependentRenderer.VirtualBounds, new PongRectangleRandomSource(new LinearDice(), 2));
    }

    public static void StartClearNewGame(GlobeProvider globeProvider, IScreen currentScreen,
        IScreenManager screenManager, ICampaignGenerator campaignGenerator)
    {
        globeProvider.GenerateNew();

        var availableLaunches = campaignGenerator.CreateSet(globeProvider.Globe);

        screenManager.ExecuteTransition(
            currentScreen,
            ScreenTransition.CommandCenter,
            new CommandCenterScreenTransitionArguments(availableLaunches));
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

        spriteBatch.Draw(_uiContentStorage.GetTitleBackgroundTexture(), _bgPong.GetRects()[0], Color.White);
        spriteBatch.Draw(_uiContentStorage.GetModalShadowTexture(),
            new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Center.X - 128, 0, 256, 480),
            sourceRectangle: null, Color.Lerp(Color.White, Color.Transparent, 0.3f));

        var heroesRect = new Rectangle(0, 0, ResolutionIndependentRenderer.VirtualBounds.Width,
            ResolutionIndependentRenderer.VirtualBounds.Height / 2);
        DrawHeroes(spriteBatch, heroesRect);

        var logoRect = new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Center.Y - 128,
            ResolutionIndependentRenderer.VirtualBounds.Width, 64);
        DrawLogo(spriteBatch, logoRect);

        var menuRect = new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Center.Y,
            ResolutionIndependentRenderer.VirtualBounds.Width, ResolutionIndependentRenderer.VirtualBounds.Height / 2);
        DrawMenu(spriteBatch, menuRect);

        if (_gameSettings.Mode.HasFlag(GameMode.Demo) && !_gameSettings.Mode.HasFlag(GameMode.Recording))
        {
            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.DemoMarkerText,
                new Vector2(
                    ResolutionIndependentRenderer.VirtualBounds.Right - 100,
                    ResolutionIndependentRenderer.VirtualBounds.Top + 10),
                Color.White);
        }

        if (!_gameSettings.Mode.HasFlag(GameMode.Recording))
        {
            var socialPosition = new Vector2(ResolutionIndependentRenderer.VirtualBounds.Right - 75,
                ResolutionIndependentRenderer.VirtualBounds.Bottom - 150);
            spriteBatch.Draw(_uiContentStorage.GetSocialTexture(), socialPosition, Color.White);
        }

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

        _bgPong.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }

    private ButtonBase? CreateLoadButtonOrNothing()
    {
        if (!_globeProvider.CheckSavesExist())
        {
            return null;
        }

        var loadGameButton = new TitleResourceTextButton(nameof(UiResource.PlayStoryButtonTitle));

        loadGameButton.OnClick += (_, _) =>
        {
            var continueDialog = new ContinueGameModal(_uiContentStorage, _resolutionIndependentRenderer,
                _globeProvider, ScreenManager, this, _campaignGenerator);
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
        ScreenManager.ExecuteTransition(this, ScreenTransition.Credits, null!);
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

    private void FreeCombatButton_OnClick(object? sender, EventArgs e)
    {
        var freeHeroes = new[]
        {
            ("swordsman", new StatValue(5)),
            ("amazon", new StatValue(3)),
            ("partisan", new StatValue(3)),
            ("robber", new StatValue(3)),
            ("monk", new StatValue(3)),
            ("guardian", new StatValue(5))
        };

        var freeMonsters = new[]
        {
            "digitalwolf",
            "corruptedbear",
            "wisp",
            "chaser",
            "aspid",
            "volkolakwarrior",
            "agressor",
            "ambushdrone",
            "automataur"
        };

        var freeLocations = new[]
        {
            LocationSids.Thicket,
            LocationSids.Desert,
            LocationSids.Monastery,
            LocationSids.Battleground,
            LocationSids.Swamp,
            LocationSids.ShipGraveyard
        };

        var heroPositions = Enumerable.Range(0, 6).Select(x => new FieldCoords(x / 3, x % 3)).ToArray();
        var monsterPositions = Enumerable.Range(0, 6).Select(x => new FieldCoords(x / 3, x % 3)).ToArray();

        var dice = new LinearDice();
        var rolledHeroes = dice.RollFromList(freeHeroes, dice.Roll(2, 4)).ToArray();
        var rolledHeroPositions = _dice.RollFromList(heroPositions, rolledHeroes.Length).ToArray();
        var heroStates = rolledHeroPositions
            .Select((position, heroIndex) => (
                HeroState.Create(rolledHeroes[heroIndex].Item1), position))
            .ToArray();
        _globeProvider.GenerateFree(heroStates.Select(x => x.Item1).ToArray());

        var rolledMonsters = _dice.RollFromList(freeMonsters, dice.Roll(2, 4)).ToArray();
        var rolledCoords = _dice.RollFromList(monsterPositions, rolledMonsters.Length).ToArray();

        var prefabs = rolledCoords.Select((t, i) => new MonsterCombatantPrefab(rolledMonsters[i], 0, t)).ToList();

        var combat =
            new CombatSource(
                prefabs.Select(x => new PerkMonsterCombatantPrefab(x, ArraySegment<ICombatantStatusFactory>.Empty))
                    .ToArray(), new CombatReward(Array.Empty<IDropTableScheme>()));
        var combatSequence = new CombatSequence
        {
            Combats = new[] { combat }
        };

        var rolledLocation = dice.RollFromList(freeLocations);

        var oneCombatNode = new GraphNode<ICampaignStageItem>(new CombatStageItem(rolledLocation, combatSequence));
        var oneCombatGraph = new DirectedGraph<ICampaignStageItem>();
        oneCombatGraph.AddNode(oneCombatNode);
        var campaignSource = new HeroCampaignLocation(rolledLocation, oneCombatGraph);
        var campaign = new HeroCampaign(heroStates, campaignSource, ArraySegment<ICampaignReward>.Empty, 1);

        ScreenManager.ExecuteTransition(
            this,
            ScreenTransition.Combat,
            new CombatScreenTransitionArguments(campaign, combatSequence, 1, false, rolledLocation, null));
    }

    private UnitName[] GetAvailableHeroes()
    {
        if (_gameSettings.Mode == GameMode.Demo)
        {
            return new[]
            {
                UnitName.Swordsman, UnitName.Robber, UnitName.Herbalist, UnitName.Assaulter, UnitName.Monk,
                UnitName.Guardian, UnitName.Hoplite
            };
        }

        var lastHeroes = GetLastHeroes(_globeProvider);

        return lastHeroes;
    }

    private static UnitName[] GetDefaultShowcaseHeroes()
    {
        return new[]
        {
            UnitName.Swordsman, UnitName.Robber, UnitName.Herbalist
        };
    }

    private static UnitName[] GetLastHeroes(GlobeProvider globeProvider)
    {
        var lastSave = globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).FirstOrDefault();

        if (lastSave is null)
        {
            return new[] { UnitName.Swordsman };
        }

        var saveData = globeProvider.GetStoredData(lastSave.FileName);

        var activeUnits = saveData.Progress.Player?.Heroes?.Where(x => x?.HeroSid != null).Select(x => x!.HeroSid!);

        if (activeUnits is null)
        {
            // Get save but player data is not available.

            return GetDefaultShowcaseHeroes();
        }

        var unitNames = activeUnits.Select(GetLastHeroName).ToArray();
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
        StartClearNewGame(_globeProvider, this, ScreenManager, _campaignGenerator);
    }
}