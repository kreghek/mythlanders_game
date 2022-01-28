using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.GameScreens.Event.Tutorial;
using Rpg.Client.GameScreens.Event.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Speech
{
    internal class SpeechScreen: GameScreenWithMenuBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public SpeechScreen(EwarGame game) : base(game)
        {
            _random = new Random();
            
            _globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = _globeProvider.Globe;
            
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            
            var combat = _globe.ActiveCombat ??
                         throw new InvalidOperationException(
                             $"{nameof(_globe.ActiveCombat)} can't be null in this screen.");

            _globeNode = combat.Node;

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);
            
            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();
            
            var data = new[] { Color.White };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);
            
            _buttons = new List<ButtonBase>();
            _textFragments = new List<TextFragment>();

            _dialogContext = new EventContext(_globe);
            
            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _dice = Game.Services.GetService<IDice>();
        }
        
        private readonly IList<ButtonBase> _buttons;
        private readonly EventContext _dialogContext;

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            DrawGameObjects(spriteBatch);
            DrawSpeaker(spriteBatch);
            DrawHud(spriteBatch, contentRect);
        }
        
        private readonly IList<TextFragment> _textFragments;
        
        private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            const int FACE_SIZE = 256;
            
            if (_textFragments.Any())
            {
                var textFragmentControl = _textFragments[_currentFragmentIndex];

                var textFragmentSize = textFragmentControl.CalculateSize();

                const int SPEECH_MARGIN = 50;
                textFragmentControl.Rect = new Rectangle(
                    new Point(FACE_SIZE, contentRectangle.Bottom - (int)textFragmentSize.Y - SPEECH_MARGIN),
                    new Point((int)textFragmentSize.X, (int)textFragmentSize.Y));
                textFragmentControl.Draw(spriteBatch);
            }

            // if (_currentFragmentIndex == _textFragments.Count - 1)
            // {
            //     var optionsStartPosition = new Vector2(textContentRect.X, lastBottomPosition.Y);
            //
            //     var index = 0;
            //     foreach (var button in _buttons)
            //     {
            //         var optionPosition = optionsStartPosition + Vector2.UnitY * index * 25;
            //         var optionButtonSize = new Point(100, 25);
            //         button.Rect = new Rectangle(optionPosition.ToPoint(), optionButtonSize);
            //         button.Draw(spriteBatch);
            //     }
            // }

            spriteBatch.End();
        }

        private int _currentFragmentIndex;
        
        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            var backgroundType = BackgroundHelper.GetBackgroundType(_globeNode.Sid);

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
        
        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        /// <summary>
        /// Event screen has no background parallax.
        /// </summary>
        private const float BG_CENTER_OFFSET_PERCENTAGE = 0;
        
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

        private void DrawSpeaker(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            var col = _frameIndex % 2;
            var row = _frameIndex / 2;

            spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(),
                new Rectangle(0, ResolutionIndependentRenderer.VirtualHeight - 256, 256, 256),
                new Rectangle(col * 256, row * 256, 256, 256),
                Color.White);

            spriteBatch.End();
        }

        private double _counter;
        private int _frameIndex;
        private readonly Random _random;
        private readonly GlobeProvider _globeProvider;
        private readonly Globe _globe;
        private readonly Texture2D _backgroundTexture;
        private readonly GlobeNode _globeNode;
        private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private readonly IEventCatalog _eventCatalog;
        private readonly IDice _dice;

        private bool _isInitialized;

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);
            
            if (!_globe.Player.HasAbility(PlayerAbility.ReadEventTutorial) &&
                !_globe.Player.HasAbility(PlayerAbility.SkipTutorials) && !_globe.CurrentEvent?.IsGameStart == true)
            {
                _globe.Player.AddPlayerAbility(PlayerAbility.ReadEventTutorial);

                var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                    ResolutionIndependentRenderer, _globe.Player);
                AddModal(tutorialModal, isLate: false);
            }
            
            if (!_isInitialized)
            {
                InitEventControls();

                _isInitialized = true;
            }
            else
            {
                UpdateBackgroundObjects(gameTime);

                UpdateHud(gameTime);
                
                UpdateSpeaker(gameTime);
            }
        }
        
        private void UpdateHud(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (_currentFragmentIndex < _textFragments.Count - 1 &&
                    !_textFragments[_currentFragmentIndex].IsComplete)
                {
                    foreach (var fragment in _textFragments)
                    {
                        fragment.MoveToCompletion();
                    }

                    _currentFragmentIndex = _textFragments.Count - 1;
                }

                return;
            }

            var currentFragment = _textFragments[_currentFragmentIndex];
            currentFragment.Update(gameTime);

            if (currentFragment.IsComplete)
            {
                if (_currentFragmentIndex < _textFragments.Count - 1)
                {
                    //TODO Make auto-move to next dialog. Make it disable in settings by default.
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        _currentFragmentIndex++;
                    }
                }
            }

            if (_currentFragmentIndex == _textFragments.Count - 1)
            {
                foreach (var button in _buttons)
                {
                    button.Update(ResolutionIndependentRenderer);
                }
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
        
        private static string GetOptionLocalizedText(EventOption option)
        {
            return PlotResources.ResourceManager.GetString($"EventOption{option.TextSid}Text") ?? option.TextSid;
        }
        
        private void InitEventControls()
        {
            _textFragments.Clear();
            _currentFragmentIndex = 0;
            foreach (var textFragment in _globe.CurrentEventNode.TextBlock.Fragments)
            {
                var texture = _uiContentStorage.GetSpeechTexture();
                if (textFragment.Speaker == UnitName.Environment)
                {
                    texture = _uiContentStorage.GetEnvSpeechTexture();
                }

                var textFragmentControl = new TextFragment(texture,
                    _uiContentStorage.GetTitlesFont(),
                    textFragment, _gameObjectContentStorage.GetUnitPortrains(),
                    _gameObjectContentStorage.GetTextSoundEffect(textFragment.Speaker));
                _textFragments.Add(textFragmentControl);
            }

            _buttons.Clear();
            foreach (var option in _globe.CurrentEventNode.Options)
            {
                var optionLocalizedText = GetOptionLocalizedText(option);
                var button = new TextButton(optionLocalizedText, _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);
                button.OnClick += (_, _) =>
                {
                    option.Aftermath?.Apply(_dialogContext);

                    if (option.IsEnd)
                    {
                        if (_globe.CurrentEventNode.CombatPosition == EventPosition.BeforeCombat)
                        {
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                        }
                        else
                        {
                            if (_globe.CurrentEvent.GoalDescription is not null)
                            {
                                _globe.Player.CurrentGoalEvent = _globe.CurrentEvent;
                            }

                            _globe.CurrentEvent = null;
                            _globe.CurrentEventNode = null;
                            _globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                            _globeProvider.StoreCurrentGlobe();
                        }
                    }
                    else
                    {
                        _isInitialized = false;
                    }
                };

                _buttons.Add(button);
            }
        }

        private void UpdateSpeaker(GameTime gameTime)
        {
            const int SPEAKER_FRAME_COUNT = 3;
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