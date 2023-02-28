using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;
using Client.GameScreens.Campaign;
using Client.GameScreens.TextDialogue.Ui;

using Core.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.GameScreens.Speech.Tutorial;
using Rpg.Client.GameScreens.Speech.Ui;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.TextDialogue;

internal class TextDialogueScreen : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 3;
    private const float BACKGROUND_LAYERS_SPEED = 0.1f;

    /// <summary>
    /// Event screen has no background parallax.
    /// </summary>
    private const float BG_CENTER_OFFSET_PERCENTAGE = 0;

    private readonly Texture2D _backgroundTexture;
    private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
    private readonly HeroCampaign _currentCampaign;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly DialogueOptions _dialogueOptions;
    private readonly DialoguePlayer _dialoguePlayer;
    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly GameSettings _gameSettings;
    private readonly LocationSid _globeLocation;
    private readonly GlobeProvider _globeProvider;
    private readonly Player _player;
    private readonly Random _random;
    private readonly IList<TextFragmentControl> _textFragments;
    private readonly IUiContentStorage _uiContentStorage;

    private double _counter;

    private int _currentFragmentIndex;

    private bool _currentTextFragmentIsReady;
    private int _frameIndex;

    private bool _isInitialized;

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
        _textFragments = new List<TextFragmentControl>();

        var dualogueContextFactory =
            new DialogueContextFactory(globe, storyPointCatalog, _player, args.DualogueEvent);
        _dialoguePlayer =
            new DialoguePlayer(args.CurrentDialogue, dualogueContextFactory);

        _eventCatalog = game.Services.GetService<IEventCatalog>();

        _dice = Game.Services.GetService<IDice>();

        _gameSettings = game.Services.GetService<GameSettings>();

        _currentCampaign = args.CurrentCampaign;

        _dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();

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

    private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
        int backgroundMaxOffset)
    {
        for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
        {
            var xFloat = backgroundStartOffset + BG_CENTER_OFFSET_PERCENTAGE * (BACKGROUND_LAYERS_COUNT - i - 1) *
                BACKGROUND_LAYERS_SPEED * backgroundMaxOffset;
            var roundedX = (int)Math.Round(xFloat);
            var position = new Vector2(roundedX, 0);

            var position3d = new Vector3(position, 0);

            var worldTransformationMatrix = Camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + position3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: matrix);

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

        if (speaker == UnitName.Environment)
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

        var col = 0;
        var row = 0;
        // var col = _frameIndex % 2;
        // var row = _frameIndex / 2;

        spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(speaker),
            new Rectangle(0, ResolutionIndependentRenderer.VirtualHeight - SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            new Rectangle(col * SPEAKER_FRAME_SIZE, row * SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            Color.White);

        spriteBatch.End();
    }

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
        int backgroundMaxOffset)
    {
        var xFloat = backgroundStartOffset +
                     -1 * BG_CENTER_OFFSET_PERCENTAGE * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
        var roundedX = (int)Math.Round(xFloat);

        var position = new Vector2(roundedX, 0);

        var position3d = new Vector3(position, 0);

        var worldTransformationMatrix = Camera.GetViewTransformationMatrix();
        worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

        var matrix = Matrix.CreateTranslation(translationVector + position3d)
                     * Matrix.CreateScale(scaleVector);

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: matrix);

        spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);

        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawGameObjects(SpriteBatch spriteBatch)
    {
        var backgroundType = BackgroundHelper.GetBackgroundType(_globeLocation);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(backgroundType);

        const int BG_START_OFFSET = -100;
        const int BG_MAX_OFFSET = 200;

        DrawBackgroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);

        DrawForegroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);

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
        _currentCampaign.CompleteCurrentStage();
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
            var textFragmentControl = new TextFragmentControl(
                textFragment,
                _gameObjectContentStorage.GetUnitPortrains(),
                _gameObjectContentStorage.GetTextSoundEffect(textFragment.Speaker),
                _dice,
                _dialogueEnvironmentManager,
                _player.StoryState);
            _textFragments.Add(textFragmentControl);
        }

        _dialogueOptions.Options.Clear();
        foreach (var option in _dialoguePlayer.CurrentOptions)
        {
            var optionButton = new DialogueOptionButton(option.TextSid);
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
        }
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

        var maxFragmentIndex = _textFragments.Count - 1;
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            if (_currentFragmentIndex <= maxFragmentIndex &&
                !_textFragments[_currentFragmentIndex].IsComplete)
            {
                foreach (var fragment in _textFragments)
                {
                    fragment.MoveToCompletion();
                }

                _currentFragmentIndex = maxFragmentIndex;
            }

            return;
        }

        var currentFragment = _textFragments[_currentFragmentIndex];
        currentFragment.Update(gameTime);

        if (currentFragment.IsComplete)
        {
            if (_currentFragmentIndex < maxFragmentIndex)
            {
                _currentTextFragmentIsReady = true;
                //TODO Make auto-move to next dialog. Make it disable in settings by default.
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _currentFragmentIndex++;
                    _currentTextFragmentIsReady = false;
                }
            }
        }

        if (_currentFragmentIndex == maxFragmentIndex && _textFragments[_currentFragmentIndex].IsComplete)
        {
            _dialogueOptions.Update(ResolutionIndependentRenderer);
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