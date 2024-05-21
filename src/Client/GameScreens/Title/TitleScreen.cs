using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Common;
using Client.GameScreens.PreHistory;
using Client.ScreenManagement;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using GameClient.Engine.RectControl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
    private readonly StateCoordinator _coordinator;
    private readonly IDice _dice;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly GameSettings _gameSettings;

    private readonly GlobeProvider _globeProvider;

    private readonly ParticleSystem _particleSystem;
    private readonly ParticleSystem[] _pulseParticleSystems;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly IDialogueResourceProvider _resourceProvider;

    private readonly SettingsModal _settingsModal;
    private readonly UnitName[] _showcaseUnits;
    private readonly IUiContentStorage _uiContentStorage;

    public TitleScreen(MythlandersGame game)
        : base(game)
    {
        _globeProvider = Game.Services.GetRequiredService<GlobeProvider>();

        _camera = Game.Services.GetRequiredService<ICamera2DAdapter>();
        _resolutionIndependentRenderer = Game.Services.GetRequiredService<IResolutionIndependentRenderer>();

        _dice = Game.Services.GetRequiredService<IDice>();
        _gameSettings = Game.Services.GetRequiredService<GameSettings>();

        var soundtrackManager = Game.Services.GetRequiredService<SoundtrackManager>();
        soundtrackManager.PlayTitleTrack();

        _uiContentStorage = game.Services.GetRequiredService<IUiContentStorage>();
        _gameObjectContentStorage = game.Services.GetRequiredService<GameObjectContentStorage>();
        _campaignGenerator = game.Services.GetRequiredService<ICampaignGenerator>();

        _coordinator = game.Services.GetRequiredService<StateCoordinator>();

        _resourceProvider = game.Services.GetRequiredService<IDialogueResourceProvider>();

        _buttons = new List<ButtonBase>();

        MakeStartGameButton();

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

    public static void StartClearNewGame(GlobeProvider globeProvider,
        IScreen currentScreen,
        IScreenManager screenManager,
        IDialogueResourceProvider dialogueResourceProvider)
    {
        globeProvider.GenerateNew();

        const string PRE_HISTORY_RESOURCE_FILE_SID = "pre-history";
        const string PRE_HISTORY_DIALOGUE_SID = PRE_HISTORY_RESOURCE_FILE_SID;

        var dialogueYaml = dialogueResourceProvider.GetResource(PRE_HISTORY_RESOURCE_FILE_SID);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var dialogueDtoDict = deserializer.Deserialize<Dictionary<string, DialogueDtoScene>>(dialogueYaml);

        var preHistoryDialogue = DialogueCatalogHelper.Create(
            PRE_HISTORY_DIALOGUE_SID, dialogueDtoDict,
            new DialogueCatalogCreationServices<PreHistoryConditionContext, PreHistoryAftermathContext>(
                new PreHistoryDialogueEnvironmentEffectCreator(),
                new PreHistoryOptionAftermathCreator(),
                new PreHistoryParagraphConditionCreator()),
            _ => ArraySegment<IDialogueParagraphCondition<PreHistoryConditionContext>>.Empty);

        screenManager.ExecuteTransition(
            currentScreen,
            ScreenTransition.PreHistory,
            new PreHistoryScreenScreenTransitionArguments(preHistoryDialogue));
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

            var demoDescriptionText = StringHelper.LineBreaking(UiResource.DemoMarkerDescription, 40);
            var demoDescriptionTextSize = _uiContentStorage.GetMainFont().MeasureString(demoDescriptionText);

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), demoDescriptionText,
                new Vector2(
                    ResolutionIndependentRenderer.VirtualBounds.Right - demoDescriptionTextSize.X -
                    ControlBase.CONTENT_MARGIN,
                    ResolutionIndependentRenderer.VirtualBounds.Top + 25),
                MythlandersColors.Description);
        }

        if (!_gameSettings.Mode.HasFlag(GameMode.Recording))
        {
            var teamLogoTexture = _uiContentStorage.GetSocialTexture();

            var teamPosition = new Vector2(
                ResolutionIndependentRenderer.VirtualBounds.Right - teamLogoTexture.Width - ControlBase.CONTENT_MARGIN,
                ResolutionIndependentRenderer.VirtualBounds.Bottom - teamLogoTexture.Height -
                ControlBase.CONTENT_MARGIN);

            spriteBatch.Draw(teamLogoTexture, teamPosition, Color.White);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        UpdateUi();

        _particleSystem.Update(gameTime);
        foreach (var particleSystem in _pulseParticleSystems)
        {
            particleSystem.Update(gameTime);
        }

        _bgPong.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }

    private ButtonBase? CreateLoadButtonOrNothing()
    {
        if (_gameSettings.Mode == GameMode.Demo)
        {
            // Do not store game in demo version.
            // Looks like game has no saves.
            return null;
        }

        if (!_globeProvider.CheckSavesExist())
        {
            return null;
        }

        var loadGameButton = new TitleResourceTextButton(nameof(UiResource.PlayStoryButtonTitle));

        loadGameButton.OnClick += (_, _) =>
        {
            var continueDialog = new ContinueGameModal(_uiContentStorage, _resolutionIndependentRenderer,
                _globeProvider, ScreenManager, this, _coordinator, _resourceProvider);
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
        //DrawMusicPulse(spriteBatch, contentRect);

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
        var lastHeroes = GetLastHeroes(_globeProvider);

        return lastHeroes;
    }

    private static UnitName[] GetDefaultShowcaseHeroes()
    {
        return new[]
        {
            UnitName.Bogatyr, UnitName.Robber, UnitName.Herbalist
        };
    }

    private static UnitName[] GetLastHeroes(GlobeProvider globeProvider)
    {
        var lastSave = globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).FirstOrDefault();

        if (lastSave is null)
        {
            return new[] { UnitName.Bogatyr };
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

    private void MakeStartGameButton()
    {
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
    }

    private void SettingsButton_OnClick(object? sender, EventArgs e)
    {
        _settingsModal.Show();
    }

    private void StartButton_OnClick(object? sender, EventArgs e)
    {
        if (_gameSettings.Mode == GameMode.Demo)
        {
            var demoModal = new DemoLimitsModal(_uiContentStorage, ResolutionIndependentRenderer);
            AddModal(demoModal, false);

            demoModal.Closed += (_, _) =>
            {
                StartClearNewGame(_globeProvider, this, ScreenManager, _resourceProvider);
            };
        }
        else
        {
            StartClearNewGame(_globeProvider, this, ScreenManager, _resourceProvider);
        }
    }

    private void UpdateUi()
    {
        if (!Game.IsActive)
        {
            return;
        }

        foreach (var button in _buttons)
        {
            button.Update(_resolutionIndependentRenderer);
        }
    }
}