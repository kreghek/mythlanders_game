using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Combat.GameObjects.Background;
using Client.GameScreens.Common;
using Client.GameScreens.TextDialogue.Tutorial;
using Client.GameScreens.TextDialogue.Ui;
using Client.ScreenManagement;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.TextDialogue;

internal class TextDialogueScreen : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 3;

    private readonly Texture2D _backgroundTexture;
    private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
    private readonly HeroCampaign _currentCampaign;
    private readonly DialogueContextFactory _dialogueContextFactory;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly DialogueOptions _dialogueOptions;
    private readonly DialoguePlayer<ParagraphConditionContext, CampaignAftermathContext> _dialoguePlayer;
    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly GameSettings _gameSettings;
    private readonly ILocationSid _globeLocation;
    private readonly GlobeProvider _globeProvider;
    private readonly Player _player;
    private readonly Random _random;
    private readonly IList<TextParagraphControl> _textFragments;
    private readonly IUiContentStorage _uiContentStorage;

    private double _counter;

    private int _currentFragmentIndex;

    private bool _currentTextFragmentIsReady;
    private int _frameIndex;

    private bool _isInitialized;

    private KeyboardState _keyboardState;

    private double _pressToContinueCounter;

    public TextDialogueScreen(TestamentGame game, TextDialogueScreenTransitionArgs args) : base(game)
    {
        _random = new Random();

        _globeProvider = game.Services.GetService<GlobeProvider>();
        var globe = _globeProvider.Globe;
        if (globe is null)
        {
            throw new InvalidOperationException();
        }

        _player = globe.Player ?? throw new InvalidOperationException();

        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
        var storyPointCatalog = game.Services.GetService<IStoryPointCatalog>();

        _globeLocation = args.Location;

        var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

        var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeLocation);

        _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
        _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

        var data = new[] { Color.White };
        _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        _backgroundTexture.SetData(data);

        _dialogueOptions = new DialogueOptions();
        _textFragments = new List<TextParagraphControl>();

        _dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();

        _dialogueContextFactory =
            new DialogueContextFactory(globe, storyPointCatalog, _player, _dialogueEnvironmentManager,
                args.DialogueEvent, args.Campaign);
        _dialoguePlayer =
            new DialoguePlayer<ParagraphConditionContext, CampaignAftermathContext>(args.CurrentDialogue,
                _dialogueContextFactory);

        _eventCatalog = game.Services.GetService<IEventCatalog>();

        _dice = Game.Services.GetService<IDice>();

        _gameSettings = game.Services.GetService<GameSettings>();

        _currentCampaign = args.Campaign;

        var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

        soundtrackManager.PlaySilence();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_isInitialized)
        {
            return;
        }

        DrawGameObjects(spriteBatch);

        if (!_dialoguePlayer.IsEnd)
        {
            DrawCurrentSpeakerPortrait(spriteBatch);

            DrawTextBlock(spriteBatch, contentRect);
        }
    }

    protected override void InitializeContent()
    {
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        CheckTutorial();

        if (!_isInitialized)
        {
            InitDialogueControls();

            _isInitialized = true;
        }
        else
        {
            UpdateBackgroundObjects(gameTime);

            UpdateHud(gameTime);

            UpdateSpeaker(gameTime);
        }

        _keyboardState = Keyboard.GetState();
    }

    private void CheckTutorial()
    {
        if (_player.HasAbility(PlayerAbility.SkipTutorials))
        {
            return;
        }

        if (_player.HasAbility(PlayerAbility.ReadEventTutorial))
        {
            return;
        }

        _player.AddPlayerAbility(PlayerAbility.ReadEventTutorial);

        var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
            ResolutionIndependentRenderer, _player);
        AddModal(tutorialModal, isLate: false);
    }

    private static UnitName ConvertSpeakerToUnitName(IDialogueSpeaker speaker)
    {
        var speakerName = speaker.ToString();
        return Enum.Parse<UnitName>(speakerName!);
    }

    private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds)
    {
        for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);

            if (i == 0 /*Cloud layer*/)
            {
                foreach (var obj in _cloudLayerObjects)
                {
                    obj.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }

    private void DrawCurrentSpeakerPortrait(SpriteBatch spriteBatch)
    {
        const int SPEAKER_FRAME_SIZE = 256;

        var currentFragment = _dialoguePlayer.CurrentTextFragments[_currentFragmentIndex];
        var speaker = currentFragment.Speaker;

        if (DialogueSpeakers.Get(UnitName.Environment) == speaker)
        {
            // This text describes environment. There is no speaker.
            return;
        }

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        var name = ConvertSpeakerToUnitName(speaker);

        spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(name),
            new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Height - SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            new Rectangle(SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            Color.White);

        spriteBatch.End();
    }

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);

        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawGameObjects(SpriteBatch spriteBatch)
    {
        var backgroundType = LocationHelper.GetLocationTheme(_globeLocation);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(backgroundType);

        DrawBackgroundLayers(spriteBatch, backgrounds);

        DrawForegroundLayers(spriteBatch, backgrounds);

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());
        spriteBatch.Draw(_backgroundTexture, ResolutionIndependentRenderer.VirtualBounds,
            Color.Lerp(Color.Transparent, Color.Black, 0.5f));
        spriteBatch.End();
    }

    private void DrawTextBlock(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        const int PORTRAIT_SIZE = 256;

        if (_textFragments.Any())
        {
            var textFragmentControl = _textFragments[_currentFragmentIndex];

            var textFragmentSize = textFragmentControl.CalculateSize().ToPoint();

            const int SPEECH_MARGIN = 50;
            var sumOptionHeight = _dialogueOptions.GetHeight();
            var fragmentHeight = textFragmentSize.Y + SPEECH_MARGIN + sumOptionHeight;
            var fragmentPosition = new Point(PORTRAIT_SIZE, contentRectangle.Bottom - fragmentHeight);
            textFragmentControl.Rect = new Rectangle(fragmentPosition, textFragmentSize);
            textFragmentControl.Draw(spriteBatch);

            if (_currentTextFragmentIsReady)
            {
                // TODO Move to separated control
                var isVisible = (float)Math.Sin(_pressToContinueCounter) > 0;

                if (isVisible)
                {
                    spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.DialogueContinueText,
                        new Vector2(PORTRAIT_SIZE, contentRectangle.Bottom - 25), Color.White);
                }
            }
        }

        if (_currentFragmentIndex == _textFragments.Count - 1 && _textFragments[_currentFragmentIndex].IsComplete)
        {
            const int OPTION_BUTTON_MARGIN = 5;
            var lastTopButtonPosition = _textFragments[_currentFragmentIndex].Rect.Bottom + OPTION_BUTTON_MARGIN;

            _dialogueOptions.Rect = new Rectangle(PORTRAIT_SIZE, lastTopButtonPosition,
                contentRectangle.Width - PORTRAIT_SIZE + 100,
                contentRectangle.Height - lastTopButtonPosition + 100);
            _dialogueOptions.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void HandleDialogueEnd()
    {
        _globeProvider.Globe.Update(_dice, _eventCatalog);
        _dialogueEnvironmentManager.Clean();
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_currentCampaign));

        if (_gameSettings.Mode == GameMode.Full)
        {
            _globeProvider.StoreCurrentGlobe();
        }
    }

    private void InitDialogueControls()
    {
        _textFragments.Clear();
        _currentFragmentIndex = 0;
        foreach (var textFragment in _dialoguePlayer.CurrentTextFragments)
        {
            var name = ConvertSpeakerToUnitName(textFragment.Speaker);
            var textFragmentControl = new TextParagraphControl(
                textFragment,
                _gameObjectContentStorage.GetTextSoundEffect(name),
                _dice,
                _dialogueContextFactory.CreateAftermathContext(),
                _player.StoryState);
            _textFragments.Add(textFragmentControl);
        }

        var optionNumber = 1;
        _dialogueOptions.Options.Clear();
        foreach (var option in _dialoguePlayer.CurrentOptions)
        {
            var optionButton = new DialogueOptionButton(optionNumber, option.TextSid);
            optionButton.OnClick += (_, _) =>
            {
                _dialoguePlayer.SelectOption(option);

                if (_dialoguePlayer.IsEnd)
                {
                    HandleDialogueEnd();
                }
                else
                {
                    _isInitialized = false;
                }
            };

            _dialogueOptions.Options.Add(optionButton);
            optionNumber++;
        }
    }

    private bool IsKeyPressed(Keys checkKey)
    {
        return Keyboard.GetState().IsKeyUp(checkKey) && _keyboardState.IsKeyDown(checkKey);
    }

    private void UpdateBackgroundObjects(GameTime gameTime)
    {
        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Update(gameTime);
        }

        foreach (var obj in _cloudLayerObjects)
        {
            obj.Update(gameTime);
        }
    }

    private void UpdateHud(GameTime gameTime)
    {
        _pressToContinueCounter += gameTime.ElapsedGameTime.TotalSeconds * 10f;

        var currentFragment = _textFragments[_currentFragmentIndex];
        currentFragment.Update(gameTime);

        var maxFragmentIndex = _textFragments.Count - 1;
        if (IsKeyPressed(Keys.Space) && !_textFragments[_currentFragmentIndex].IsComplete)
        {
            currentFragment.FastComplete();

            return;
        }

        if (currentFragment.IsComplete)
        {
            if (_currentFragmentIndex < maxFragmentIndex)
            {
                _currentTextFragmentIsReady = true;
                //TODO Make auto-move to next dialog. Make it disabled in settings by default.
                if (IsKeyPressed(Keys.Space))
                {
                    _currentFragmentIndex++;
                    _currentTextFragmentIsReady = false;
                }
            }
        }

        if (_currentFragmentIndex == maxFragmentIndex && _textFragments[_currentFragmentIndex].IsComplete)
        {
            _dialogueOptions.Update(ResolutionIndependentRenderer);

            if (IsKeyPressed(Keys.D1))
            {
                _dialogueOptions.SelectOption(1);
            }
            else if (IsKeyPressed(Keys.D2))
            {
                _dialogueOptions.SelectOption(2);
            }
            else if (IsKeyPressed(Keys.D3))
            {
                _dialogueOptions.SelectOption(3);
            }
            else if (IsKeyPressed(Keys.D4))
            {
                _dialogueOptions.SelectOption(4);
            }
        }
    }

    private void UpdateSpeaker(GameTime gameTime)
    {
        const int SPEAKER_FRAME_COUNT = 4;
        const double SPEAKER_FRAME_DURATION = 0.25;

        var currentFragment = _textFragments[_currentFragmentIndex];
        if (!currentFragment.IsComplete)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > SPEAKER_FRAME_DURATION)
            {
                _frameIndex = _random.Next(SPEAKER_FRAME_COUNT);
                if (_frameIndex > SPEAKER_FRAME_COUNT - 1)
                {
                    _frameIndex = 0;
                }

                _counter = 0;
            }
        }
        else
        {
            _frameIndex = 0;
        }
    }
}