using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Event
{
    public sealed class EventScreen : GameScreenBase
    {
        private readonly IList<BaseButton> _buttons;

        private readonly IDialogContext _dialogContext;

        private readonly Globe _globe;

        private readonly IUiContentStorage _uiContentStorage;

        private DialogNode? _currentDialogNode;

        private bool _isInitialized;

        public EventScreen(IScreenManager screenManager, Globe globe, IUiContentStorage uiContentStorage,
            IDialogContext dialogContext)
            : base(screenManager)
        {
            _globe = globe;

            _uiContentStorage = uiContentStorage;

            _buttons = new List<BaseButton>();

            _dialogContext = dialogContext;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_isInitialized)
                return;

            spriteBatch.Begin();

            _currentDialogNode = _globe.AvailableDialog.StartNode;

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _currentDialogNode.Text, Vector2.Zero, Color.White);

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(0, 100 + index * 25, 100, 20);
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
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
                    var button = new TextBaseButton(
                        option.Text,
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(),
                        Rectangle.Empty);
                    button.OnClick += (s, e) =>
                    {
                        if (option.Aftermath is not null)
                            option.Aftermath.Apply(_dialogContext);

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
        }
    }
}