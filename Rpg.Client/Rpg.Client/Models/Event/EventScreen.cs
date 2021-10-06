﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.Tutorial;
using Rpg.Client.Models.Common;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Event
{
    internal sealed class EventScreen : GameScreenBase
    {
        private const int TEXT_MARGIN = 10;
        private const int OPTIONS_BLOCK_MARGIN = 10;
        private static bool _tutorial;
        private readonly IList<ButtonBase> _buttons;
        private readonly EventContext _dialogContext;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly IUiContentStorage _uiContentStorage;
        private EventNode _currentDialogNode;

        private bool _isInitialized;

        public EventScreen(EwarGame game) : base(game)
        {
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            _globe = game.Services.GetService<GlobeProvider>().Globe;

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _currentDialogNode = _globe.CurrentEventNode ??
                                 throw new InvalidOperationException(
                                     "The screen was started before CurrentEventNode was assigned.");

            _buttons = new List<ButtonBase>();

            _dialogContext = new EventContext(_globe);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (!_isInitialized)
            {
                return;
            }

            var font = _uiContentStorage.GetMainFont();

            spriteBatch.Begin();

            var textRect = new Rectangle(0, 0, 400, 350);
            var textContentRect = new Rectangle(
                Game.GraphicsDevice.Viewport.Bounds.Center.X - textRect.Center.X,
                Game.GraphicsDevice.Viewport.Bounds.Center.Y - textRect.Center.Y,
                textRect.Width,
                textRect.Height);

            var startPosition = textContentRect.Location.ToVector2();
            var bottomPosition = startPosition;

            for (var fragmentIndex = 0; fragmentIndex < _currentDialogNode.TextBlock.Fragments.Count; fragmentIndex++)
            {
                var fragment = _currentDialogNode.TextBlock.Fragments[fragmentIndex];
                var localizedSpeakerName = GetSpeaker(fragment.Speaker);
                var localizedSpeakerText = GetLocalizedText(fragment.TextSid);
                var speakerTextSize = font.MeasureString(localizedSpeakerText);

                var rowPosition = bottomPosition;

                var speakerNamePosition = rowPosition;
                if (localizedSpeakerName is not null)
                {
                    var portrainSourceRect = GetUnitPortrainRect(fragment.Speaker);
                    spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(),
                        rowPosition + (Vector2.UnitX * (100 - 32) / 2), portrainSourceRect, Color.White);
                    spriteBatch.DrawString(font, localizedSpeakerName, speakerNamePosition + Vector2.UnitY * 32,
                        Color.White);
                }

                var speakerTextPosition =
                    localizedSpeakerName is not null ? rowPosition + (Vector2.UnitX * 100) : rowPosition;
                var maxTextBlockWidth = Math.Max(textContentRect.Width - (localizedSpeakerName is not null ? 100 : 0),
                    (int)speakerTextSize.X + TEXT_MARGIN * 2);
                var normalizedSpeakerTextSize = new Point(maxTextBlockWidth, (int)speakerTextSize.Y + TEXT_MARGIN * 2);
                spriteBatch.Draw(_uiContentStorage.GetButtonTexture(),
                    new Rectangle(speakerTextPosition.ToPoint(), normalizedSpeakerTextSize), Color.White);
                spriteBatch.DrawString(font, localizedSpeakerText,
                    speakerTextPosition + new Vector2(TEXT_MARGIN, TEXT_MARGIN), Color.White);

                var textSize = font.MeasureString(localizedSpeakerText);

                bottomPosition = new Vector2(startPosition.X,
                    Math.Max((speakerTextPosition + textSize).Y + TEXT_MARGIN * 2, 32 + 10));
            }

            var optionsStartPosition = new Vector2(textContentRect.X, bottomPosition.Y + OPTIONS_BLOCK_MARGIN);

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

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_tutorial)
            {
                _tutorial = true;

                var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
                    Game.GraphicsDevice);
                AddModal(tutorialModal, isLate: false);
            }

            if (_isInitialized)
            {
                foreach (var button in _buttons)
                {
                    button.Update();
                }
            }
            else
            {
                _buttons.Clear();
                foreach (var option in _currentDialogNode.Options)
                {
                    var button = new TextButton(option.TextSid, _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), Rectangle.Empty);
                    button.OnClick += (s, e) =>
                    {
                        if (option.Aftermath is not null)
                        {
                            option.Aftermath.Apply(_dialogContext);
                        }

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

                _isInitialized = true;
            }
        }

        private static string GetLocalizedText(string textSid)
        {
            var rm = new ResourceManager(typeof(UiResource));
            var text = rm.GetString($"EventPlot{textSid}");

            Debug.Assert(text is not null, "Localize every text in events.");

            return text ?? textSid;
        }

        private static string? GetSpeaker(UnitName speaker)
        {
            if (speaker == UnitName.Environment)
            {
                return null;
            }

            if (speaker == UnitName.Undefined)
            {
                Debug.Fail("Speaker is undefined.");
                return null;
            }

            var rm = new ResourceManager(typeof(UiResource));
            var text = rm.GetString($"UnitName{speaker}");

            Debug.Assert(text is not null, "Speaker localiztion must be defined.");
            if (text is not null)
            {
                return text;
            }

            return speaker.ToString();
        }

        private static Rectangle GetUnitPortrainRect(UnitName speaker)
        {
            switch (speaker)
            {
                case UnitName.Hq:
                    return new Rectangle(0, 0, 32, 32);

                case UnitName.Berimir:
                    return new Rectangle(32, 0, 32, 32);

                case UnitName.Hawk:
                    return new Rectangle(0, 32, 32, 32);

                case UnitName.Oldman:
                    return new Rectangle(32, 32, 32, 32);

                case UnitName.GuardianWoman:
                    return new Rectangle(32, 64, 32, 32);

                default:
                    return Rectangle.Empty;
            }
        }
    }
}