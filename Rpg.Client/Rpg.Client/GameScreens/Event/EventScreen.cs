using System;
using System.Collections.Generic;

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

namespace Rpg.Client.GameScreens.Event
{
    internal sealed class EventScreen : GameScreenWithMenuBase
    {
        private const int TEXT_MARGIN = 10;

        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;

        /// <summary>
        /// Event screen has no background paralax.
        /// </summary>
        private const float BG_CENTER_OFFSET_PERCENTAGE = 0;

        private readonly Texture2D _backgroundTexture;
        private readonly IList<ButtonBase> _buttons;

        private readonly Camera2D _camera;
        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly EventContext _dialogContext;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeProvider _globeProvider;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IList<TextFragment> _textFragments;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private EventNode _currentDialogNode;

        private int _currentFragmentIndex;

        private bool _isInitialized;

        public EventScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

            _globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = _globeProvider.Globe;

            _currentDialogNode = _globe.CurrentEventNode ??
                                 throw new InvalidOperationException(
                                     "The screen was started before CurrentEventNode was assigned.");

            if (_currentDialogNode.CombatPosition == EventPosition.BeforeCombat)
            {
                soundtrackManager.PlayBattleTrack(_globe.CurrentBiome.Type);
            }
            else
            {
                soundtrackManager.PlayMapTrack();
            }

            _camera = Game.Services.GetService<Camera2D>();

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _dice = Game.Services.GetService<IDice>();

            _buttons = new List<ButtonBase>();
            _textFragments = new List<TextFragment>();

            _dialogContext = new EventContext(_globe);

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
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            if (!_isInitialized)
            {
                return;
            }

            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch, contentRectangle);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (!_globe.Player.HasAbility(PlayerAbility.ReadEventTutorial) &&
                !_globe.Player.HasAbility(PlayerAbility.SkipTutorials) && !_globe.CurrentEvent?.IsGameStart == true)
            {
                _globe.Player.AddPlayerAbility(PlayerAbility.ReadEventTutorial);

                var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                    _resolutionIndependentRenderer, _globe.Player);
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
            }
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

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
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

        private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            var xFloat = backgroundStartOffset +
                         -1 * BG_CENTER_OFFSET_PERCENTAGE * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
            var roundedX = (int)Math.Round(xFloat);

            var position = new Vector2(roundedX, 0);

            var position3d = new Vector3(position, 0);

            var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
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
                transformMatrix: _camera.GetViewTransformationMatrix());
            spriteBatch.Draw(_backgroundTexture, _resolutionIndependentRenderer.VirtualBounds,
                Color.Lerp(Color.Transparent, Color.Black, 0.5f));
            spriteBatch.End();
        }

        private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            var textRect = new Rectangle(0, 0, 400, 350);
            var textContentRect = new Rectangle(
                contentRectangle.Center.X - textRect.Center.X,
                contentRectangle.Center.Y - textRect.Center.Y,
                textRect.Width,
                textRect.Height);

            var startPosition = textContentRect.Location.ToVector2();
            var lastBottomPosition = startPosition;

            for (var fragmentIndex = 0; fragmentIndex <= _currentFragmentIndex; fragmentIndex++)
            {
                var textFragmentControl = _textFragments[fragmentIndex];

                var textFragmentSize = textFragmentControl.CalculateSize();
                textFragmentControl.Rect = new Rectangle(lastBottomPosition.ToPoint(),
                    new Point((int)textFragmentSize.X, (int)textFragmentSize.Y));
                textFragmentControl.Draw(spriteBatch);

                lastBottomPosition = new Vector2(textContentRect.X, textFragmentControl.Rect.Bottom + TEXT_MARGIN);
            }

            if (_currentFragmentIndex == _textFragments.Count - 1)
            {
                var optionsStartPosition = new Vector2(textContentRect.X, lastBottomPosition.Y);

                var index = 0;
                foreach (var button in _buttons)
                {
                    var optionPosition = optionsStartPosition + Vector2.UnitY * index * 25;
                    var optionButtonSize = new Point(100, 25);
                    button.Rect = new Rectangle(optionPosition.ToPoint(), optionButtonSize);
                    button.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private static string GetOptionLocalizedText(EventOption option)
        {
            return PlotResources.ResourceManager.GetString($"EventOption{option.TextSid}Text") ?? option.TextSid;
        }

        private void InitEventControls()
        {
            _textFragments.Clear();
            _currentFragmentIndex = 0;
            foreach (var textFragment in _currentDialogNode.TextBlock.Fragments)
            {
                var texture = _uiContentStorage.GetSpeechTexture();
                if (textFragment.Speaker == UnitName.Environment)
                {
                    texture = _uiContentStorage.GetEnvSpeechTexture();
                }

                var textFragmentControl = new TextFragment(texture,
                    _uiContentStorage.GetMainFont(),
                    textFragment, _gameObjectContentStorage.GetUnitPortrains(),
                    _gameObjectContentStorage.GetTextSoundEffect(textFragment.Speaker));
                _textFragments.Add(textFragmentControl);
            }

            _buttons.Clear();
            foreach (var option in _currentDialogNode.Options)
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
                        _currentDialogNode = option.Next;
                        _isInitialized = false;
                    }
                };

                _buttons.Add(button);
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
                    _currentFragmentIndex++;
                }
            }

            if (_currentFragmentIndex == _textFragments.Count - 1)
            {
                foreach (var button in _buttons)
                {
                    button.Update(_resolutionIndependentRenderer);
                }
            }
        }
    }
}