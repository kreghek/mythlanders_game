using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;

namespace Rpg.Client.ScreenManagement
{
    internal abstract class GameScreenWithMenuBase : GameScreenBase
    {
        private readonly SettingsModal _settingsModal;

        private KeyboardState _lastKeyboardState;

        protected GameScreenWithMenuBase(EwarGame game) : base(game)
        {
            var uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _settingsModal = new SettingsModal(uiContentStorage, _resolutionIndependentRenderer, Game, this);
            AddModal(_settingsModal, isLate: true);
        }

        private bool _menuCreated;
        private IList<ButtonBase>? _menuButtons;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;

        protected override void UpdateContent(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (_lastKeyboardState.IsKeyDown(Keys.F12) && keyboardState.IsKeyUp(Keys.F12))
            {
                _settingsModal.Show();
            }

            _lastKeyboardState = keyboardState;

            if (!_menuCreated)
            {
                _menuButtons = CreateMenu();
                _menuCreated = true;
            }
            else
            {
                UpdateMenu();
            }
        }

        protected abstract IList<ButtonBase> CreateMenu();

        private void UpdateMenu()
        {
            if (_menuButtons is null || !_menuButtons.Any())
            {
                return;
            }

            foreach (var menuButton in _menuButtons)
            {
                menuButton.Update(_resolutionIndependentRenderer);
            }
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            const int MENU_HEIGHT = 20;
            if (_menuButtons is not null && _menuButtons.Any())
            {
                var menuRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location,
                    new Point(ResolutionIndependentRenderer.VirtualBounds.Width, MENU_HEIGHT));


                var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
                    ResolutionIndependentRenderer.VirtualBounds.Location.Y + MENU_HEIGHT,
                    ResolutionIndependentRenderer.VirtualBounds.Width,
                    ResolutionIndependentRenderer.VirtualBounds.Height - MENU_HEIGHT);

                DrawContentWithoutMenu(spriteBatch, contentRect);

                DrawMenu(spriteBatch, menuRect, _menuButtons);
            }
            else
            {
                DrawContentWithoutMenu(spriteBatch, ResolutionIndependentRenderer.VirtualBounds);
            }
        }

        protected abstract void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect);

        private void DrawMenu(SpriteBatch spriteBatch, Rectangle menuRect, IList<ButtonBase> menuButtons)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            for (var index = 0; index < menuButtons.Count; index++)
            {
                var menuButton = menuButtons[index];

                menuButton.Rect = new Rectangle((5 + 100) * index, 0, 100, menuRect.Height);

                menuButton.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}