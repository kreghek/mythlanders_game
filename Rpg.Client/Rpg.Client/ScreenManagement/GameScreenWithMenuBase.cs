using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;
using Rpg.Client.Models.Common;

namespace Rpg.Client.ScreenManagement
{
    internal abstract class GameScreenWithMenuBase : GameScreenBase
    {
        private readonly SettingsModal _settingsModal;

        private KeyboardState _lastKeyboardState;

        protected GameScreenWithMenuBase(EwarGame game) : base(game)
        {
            var uiContentStorage = game.Services.GetService<IUiContentStorage>();
            var resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _settingsModal = new SettingsModal(uiContentStorage, resolutionIndependentRenderer, Game);
            AddModal(_settingsModal, isLate: true);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (_lastKeyboardState.IsKeyDown(Keys.F12) && keyboardState.IsKeyUp(Keys.F12))
            {
                _settingsModal.Show();
            }

            _lastKeyboardState = keyboardState;
        }
    }
}