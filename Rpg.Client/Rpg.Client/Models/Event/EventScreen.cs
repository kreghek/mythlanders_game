﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.Tutorial;
using Rpg.Client.Models.Combat.GameObjects.Background;
using Rpg.Client.Models.Common;
using Rpg.Client.Models.Event.Tutorial;
using Rpg.Client.Models.Event.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.Models.Event
{
    internal sealed class EventScreen : GameScreenWithMenuBase
    {
        private const int TEXT_MARGIN = 10;

        private const int BACKGROUND_LAYERS_COUNT = 3;
        private const float BACKGROUND_LAYERS_SPEED = 0.1f;
        private static bool _tutorial;
        private readonly IList<ButtonBase> _buttons;

        private readonly Camera2D _camera;

        private readonly IReadOnlyCollection<IBackgroundObject> _cloudLayerObjects;
        private readonly EventContext _dialogContext;
        private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IList<TextFragment> _textFragments;
        private readonly IUiContentStorage _uiContentStorage;
        private Texture2D _backgroundTexture;
        private float _bgCenterOffsetPercentage;
        private EventNode _currentDialogNode;

        private bool _isInitialized;

        public EventScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            _camera = Game.Services.GetService<Camera2D>();

            _globe = game.Services.GetService<GlobeProvider>().Globe;

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();

            _currentDialogNode = _globe.CurrentEventNode ??
                                 throw new InvalidOperationException(
                                     "The screen was started before CurrentEventNode was assigned.");

            _buttons = new List<ButtonBase>();
            _textFragments = new List<TextFragment>();

            _dialogContext = new EventContext(_globe);

            var _combat = _globe.ActiveCombat ??
                          throw new InvalidOperationException(
                              $"{nameof(_globe.ActiveCombat)} can't be null in this screen.");

            _globeNode = _combat.Node;

            var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

            var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeNode.Sid);

            _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
            _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

            var data = new[] { Color.White };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (!_isInitialized)
            {
                return;
            }

            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);
            if (!_tutorial && !_globe.CurrentEvent?.IsGameStart == true)
            {
                _tutorial = true;

                var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                    _resolutionIndependentRenderer);
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

                UpdateHud();
            }
        }

        private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
            int backgroundMaxOffset)
        {
            for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
            {
                var xFloat = backgroundStartOffset + _bgCenterOffsetPercentage * (BACKGROUND_LAYERS_COUNT - i - 1) *
                    BACKGROUND_LAYERS_SPEED * backgroundMaxOffset;
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
                         -1 * _bgCenterOffsetPercentage * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
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

        private void DrawHud(SpriteBatch spriteBatch)
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
                _resolutionIndependentRenderer.VirtualBounds.Center.X - textRect.Center.X,
                _resolutionIndependentRenderer.VirtualBounds.Center.Y - textRect.Center.Y,
                textRect.Width,
                textRect.Height);

            var startPosition = textContentRect.Location.ToVector2();
            var lastBottomPosition = startPosition;

            for (var fragmentIndex = 0; fragmentIndex < _textFragments.Count; fragmentIndex++)
            {
                var textFragmentControl = _textFragments[fragmentIndex];
                var textFragmentSize = textFragmentControl.CalculateSize();
                textFragmentControl.Rect = new Rectangle(lastBottomPosition.ToPoint(),
                    new Point(textContentRect.Width, (int)textFragmentSize.Y));
                textFragmentControl.Draw(spriteBatch);

                lastBottomPosition = new Vector2(textContentRect.X, textFragmentControl.Rect.Bottom + TEXT_MARGIN);
            }

            var optionsStartPosition = new Vector2(textContentRect.X, lastBottomPosition.Y);

            var index = 0;
            foreach (var button in _buttons)
            {
                var optionPosition = optionsStartPosition + Vector2.UnitY * index * 25;
                var optionButtonSize = new Point(100, 25);
                button.Rect = new Rectangle(optionPosition.ToPoint(), optionButtonSize);
                button.Draw(spriteBatch);
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
            foreach (var textFragment in _currentDialogNode.TextBlock.Fragments)
            {
                var textFragmentControl = new TextFragment(_uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(),
                    textFragment, _gameObjectContentStorage.GetUnitPortrains());
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
                            _globe.CurrentEvent = null;
                            _globe.CurrentEventNode = null;
                            _globe.UpdateNodes(Game.Services.GetService<IDice>());
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
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

        private void UpdateHud()
        {
            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }
    }
}