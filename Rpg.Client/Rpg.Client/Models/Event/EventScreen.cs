using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Event
{
    internal sealed class EventScreen : GameScreenBase
    {
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

            _currentDialogNode = _globe.AvailableDialog.StartNode;

            _buttons = new List<ButtonBase>();

            _dialogContext = new EventContext(_globe);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_isInitialized)
            {
                return;
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _currentDialogNode.Text, Vector2.Zero, Color.White);

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(0, 100 + index * 25, 100, 20);
                button.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
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
    }
}