using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.GameScreens.Speech.Tutorial;
using Rpg.Client.GameScreens.Speech.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Speech
{
    internal class SpeechScreen : GameScreenWithMenuBase
    {
        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        /// <summary>
        /// Event screen has no background parallax.
        /// </summary>
        private const float BG_CENTER_OFFSET_PERCENTAGE = 0;

        private readonly bool _areCombatsNext;

        private readonly Texture2D _backgroundTexture;
        private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
        private readonly CombatScreenTransitionArguments? _combatScreenArgs;
        private readonly DialoguePlayer _dialoguePlayer;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly GlobeNode _globeLocation;
        private readonly GlobeProvider _globeProvider;
        private readonly bool _isFirstDialogue;
        private readonly IList<DialogueOptionButton> _optionButtons;
        private readonly Player _player;
        private readonly Random _random;
        private readonly GameSettings _settings;

        private readonly IList<TextFragment> _textFragments;
        private readonly IUiContentStorage _uiContentStorage;

        private double _counter;

        private int _currentFragmentIndex;

        private bool _currentTextFragmentIsReady;
        private int _frameIndex;

        private bool _isInitialized;

        private double _pressToContinueCounter;

        public SpeechScreen(EwarGame game, SpeechScreenTransitionArgs args) : base(game)
        {
            _random = new Random();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = _globeProvider.Globe;
            if (_globe is null)
            {
                throw new InvalidOperationException();
            }

            _player = _globe.Player ?? throw new InvalidOperationException();

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            var storyPointCatalog = game.Services.GetService<IStoryPointCatalog>();

            _globeLocation = args.Location;

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeLocation.Sid);

            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

            var data = new[] { Color.White };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);

            _optionButtons = new List<DialogueOptionButton>();
            _textFragments = new List<TextFragment>();

            var dualogueContextFactory = new DialogueContextFactory(_globe, storyPointCatalog, _player);
            _dialoguePlayer =
                new DialoguePlayer(args.CurrentDialogue, dualogueContextFactory);
            _isFirstDialogue = args.IsStartDialogueEvent;

            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _dice = Game.Services.GetService<IDice>();

            _settings = game.Services.GetService<GameSettings>();

            var endData = DetectCombatNext(args);
            _areCombatsNext = endData.areCombatsNext;
            if (_areCombatsNext)
            {
                _combatScreenArgs = endData.combatScreenArgs;
            }

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            if (args.IsCombatPreparingDialogue)
            {
                soundtrackManager.PlayCombatTrack(args.Location.BiomeType);
            }
            else
            {
                soundtrackManager.PlayMapTrack();
            }
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

            if (_isFirstDialogue)
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

        private (bool areCombatsNext, CombatScreenTransitionArguments? combatScreenArgs) DetectCombatNext(
            SpeechScreenTransitionArgs args)
        {
            if (args.NextCombats is not null)
            {
                var combatScreenTransitionArgs = new CombatScreenTransitionArguments
                {
                    Location = args.Location,
                    CombatSequence = args.NextCombats,
                    IsAutoplay = false,
                    VictoryDialogue = args.CombatVictoryDialogue,
                    VictoryDialogueIsStartEvent = args.IsStartDialogueEvent
                };

                return new(true, combatScreenTransitionArgs);
            }

            return new(false, null);
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
            var backgroundType = BackgroundHelper.GetBackgroundType(_globeLocation.Sid);

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
            const int OPTION_BUTTON_MARGIN = 5;

            static Point CalcOptionButtonSize(DialogueOptionButton button)
            {
                var contentSize = button.GetContentSize();
                return (contentSize + Vector2.One * ControlBase.CONTENT_MARGIN + Vector2.UnitY * OPTION_BUTTON_MARGIN)
                    .ToPoint();
            }

            if (_textFragments.Any())
            {
                var textFragmentControl = _textFragments[_currentFragmentIndex];

                var textFragmentSize = textFragmentControl.CalculateSize().ToPoint();

                const int SPEECH_MARGIN = 50;
                var sumOptionHeight = _optionButtons.Sum(x => CalcOptionButtonSize(x).Y) + OPTION_BUTTON_MARGIN;
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
                var lastTopButtonPosition = _textFragments[_currentFragmentIndex].Rect.Bottom + OPTION_BUTTON_MARGIN;
                foreach (var button in _optionButtons)
                {
                    var optionButtonSize = CalcOptionButtonSize(button);
                    var optionPosition = new Vector2(PORTRAIT_SIZE, lastTopButtonPosition).ToPoint();
                    button.Rect = new Rectangle(optionPosition, optionButtonSize);
                    button.Draw(spriteBatch);

                    lastTopButtonPosition += optionButtonSize.Y;
                }
            }

            spriteBatch.End();
        }

        private void HandleDialogueEnd()
        {
            if (_areCombatsNext)
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Combat, _combatScreenArgs!);
            }
            else
            {
                _globe.Update(_dice, _eventCatalog);
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map, null);

                if (_settings.Mode == GameMode.Full)
                {
                    _globeProvider.StoreCurrentGlobe();
                }
            }
        }

        private void InitDialogueControls()
        {
            _textFragments.Clear();
            _currentFragmentIndex = 0;
            foreach (var textFragment in _dialoguePlayer.CurrentTextFragments)
            {
                var speechTexture = _uiContentStorage.GetSpeechTexture();
                if (textFragment.Speaker == UnitName.Environment)
                {
                    speechTexture = _uiContentStorage.GetEnvSpeechTexture();
                }

                var textFragmentControl = new TextFragment(
                    speechTexture,
                    _uiContentStorage.GetTitlesFont(),
                    textFragment,
                    _gameObjectContentStorage.GetUnitPortrains(),
                    _gameObjectContentStorage.GetTextSoundEffect(textFragment.Speaker),
                    _dice);
                _textFragments.Add(textFragmentControl);
            }

            _optionButtons.Clear();
            foreach (var option in _dialoguePlayer.CurrentOptions)
            {
                var optionButton = new DialogueOptionButton(option.TextSid, _uiContentStorage.GetSpeechTexture(),
                    _uiContentStorage.GetTitlesFont());
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

                _optionButtons.Add(optionButton);
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
                foreach (var button in _optionButtons)
                {
                    button.Update(ResolutionIndependentRenderer);
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
}