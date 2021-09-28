using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Event
{
    internal sealed class EventScreen : GameScreenBase
    {
        private const int ROW_SIZE = 10;
        private readonly IList<ButtonBase> _buttons;
        private readonly EventContext _dialogContext;
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

            _currentDialogNode = _globe.AvailableDialog.BeforeCombatStartNode;

            _buttons = new List<ButtonBase>();

            _dialogContext = new EventContext(_globe);
        }

        public override void Update(GameTime gameTime)
        {
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
                    var button = new TextButton(option.Text, _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), Rectangle.Empty);
                    button.OnClick += (s, e) =>
                    {
                        if (option.Aftermath is not null)
                        {
                            option.Aftermath.Apply(_dialogContext);
                        }

                        if (option.IsEnd)
                        {
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
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

            base.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            if (!_isInitialized)
            {
                return;
            }

            spriteBatch.Begin();

            var startPosition = Vector2.Zero;

            for (var fragmentIndex = 0; fragmentIndex < _currentDialogNode.TextBlock.Fragments.Count; fragmentIndex++)
            {
                var fragment = _currentDialogNode.TextBlock.Fragments[fragmentIndex];
                var localizedSpeakerName = GetSpeaker(fragment.Speaker);
                var localizedSpeakerText = GetLocalizedText(fragment.TextSid);

                var rowPosition = startPosition + (Vector2.UnitY * ROW_SIZE * fragmentIndex);

                var speakerNamePosition = startPosition + rowPosition;
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedSpeakerName, speakerNamePosition, Color.White);

                var speakerTextPosition = startPosition + rowPosition + (Vector2.UnitX * 100);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedSpeakerText, speakerTextPosition, Color.White);
            }

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(0, 100 + index * 25, 100, 20);
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private static string GetLocalizedText(string textSid)
        {
            var rm = new ResourceManager(typeof(UiResource));
            var text = rm.GetString($"EventPlot{textSid}");

            Debug.Assert(text is not null, "Localize every text in events.");

            return text ?? textSid;
        }

        private static string GetSpeaker(EventSpeaker speaker)
        {
            if (speaker == EventSpeaker.Environment)
            {
                return string.Empty;
            }

            if (speaker == EventSpeaker.Undefined)
            {
                Debug.Fail("Speaker is undefined.");
                return string.Empty;
            }

            return speaker.ToString();
        }
    }
}