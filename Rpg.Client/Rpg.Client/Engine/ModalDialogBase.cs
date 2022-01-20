using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Client.Engine
{
    internal enum ModalTopSymbol
    {
        Gears
    }

    internal abstract class ModalDialogBase : IModalWindow
    {
        private const int CLOSE_BUTTON_SIZE = 16;
        private const int CLOSE_BUTTON_PADDING = 3;

        private const int MODAL_WIDTH = 400;
        private const int MODAL_HEIGHT = 300;
        private const int MODAL_CONTENT_MARGIN = 9;
        private const int MODAL_HEADER_HEIGHT = 15;
        private readonly Texture2D _backgroundBottomTexture;

        private readonly Texture2D _backgroundTopTexture;
        private readonly Texture2D _topSymbolTexture;
        private readonly TextButton _closeButton;
        private readonly Rectangle _dialogRect;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly Texture2D _shadowTexture;
        public EventHandler? Closed;

        protected ModalDialogBase(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;

            _shadowTexture = uiContentStorage.GetModalShadowTexture();
            _backgroundTopTexture = uiContentStorage.GetModalTopTextures()[0];
            _backgroundBottomTexture = uiContentStorage.GetModalBottomTextures()[0];
            _topSymbolTexture = uiContentStorage.GetModalTopSymbolTextures();

            _dialogRect = new Rectangle(
                (_resolutionIndependentRenderer.VirtualWidth / 2) - (MODAL_WIDTH / 2),
                (_resolutionIndependentRenderer.VirtualHeight / 2) - (MODAL_HEIGHT / 2),
                MODAL_WIDTH,
                MODAL_HEIGHT);

            _closeButton = new TextButton("X", uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont(),
                new Rectangle(_dialogRect.Right - CLOSE_BUTTON_SIZE - CLOSE_BUTTON_PADDING,
                    _dialogRect.Top + CLOSE_BUTTON_PADDING, CLOSE_BUTTON_SIZE, CLOSE_BUTTON_SIZE));
            _closeButton.OnClick += CloseButton_OnClick;

            ContentRect = new Rectangle(
                _dialogRect.Left + MODAL_CONTENT_MARGIN,
                _dialogRect.Top + MODAL_CONTENT_MARGIN + MODAL_HEADER_HEIGHT,
                _dialogRect.Width - MODAL_CONTENT_MARGIN * 2,
                _dialogRect.Height - MODAL_CONTENT_MARGIN * 2 - MODAL_HEADER_HEIGHT);
        }

        protected Rectangle ContentRect { get; }

        protected abstract void DrawContent(SpriteBatch spriteBatch);

        protected virtual ModalTopSymbol? TopSymbol { get; }

        protected virtual void InitContent()
        {
            // Empty implementation to avoid empty implementation in every concrete class.
            // Some of the modals in just informational. So they are not handle any logic.
        }

        protected virtual void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            // Empty implementation to avoid empty implementation in every concrete class.
            // Some of the modals in just informational. So they are not handle any logic.
        }

        private void CloseButton_OnClick(object? sender, EventArgs e)
        {
            Close();
        }

        public bool IsVisible { get; private set; }

        public void Close()
        {
            IsVisible = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawScreenShadow(spriteBatch);

            DrawModalBorder(spriteBatch);

            DrawContent(spriteBatch);

            _closeButton.Draw(spriteBatch);
        }

        private void DrawModalBorder(SpriteBatch spriteBatch)
        {
            const int MODAL_HALF_HEIGHT = MODAL_HEIGHT / 2;

            var topRect = new Rectangle(_dialogRect.Location, new Point(MODAL_WIDTH, MODAL_HALF_HEIGHT));
            var bottomRect = new Rectangle(new Point(_dialogRect.Left, _dialogRect.Top + MODAL_HALF_HEIGHT),
                new Point(MODAL_WIDTH, MODAL_HALF_HEIGHT));
            spriteBatch.Draw(_backgroundTopTexture, topRect, Color.White);
            spriteBatch.Draw(_backgroundBottomTexture, bottomRect, Color.White);

            if (TopSymbol is not null)
            {
                var modalTopSymbolRect = GetSymbolRect(TopSymbol.Value);
                var symbolPosition = new Vector2(topRect.Center.X - SYMBOL_SIZE / 2, topRect.Top - SYMBOL_SIZE / 2);
                spriteBatch.Draw(_topSymbolTexture, symbolPosition, modalTopSymbolRect, Color.White);
            }
        }

        private const int SYMBOL_SIZE = 64;

        private Rectangle GetSymbolRect(ModalTopSymbol symbol)
        {
            var index = GetOneBasedSymbolIndex(symbol);

            const int COL_COUNT = 1;
            var col = (index - 1) % COL_COUNT;
            var row = (index - 1) / COL_COUNT;

            return new Rectangle(SYMBOL_SIZE * col, SYMBOL_SIZE * row, SYMBOL_SIZE, SYMBOL_SIZE);
        }

        private int GetOneBasedSymbolIndex(ModalTopSymbol symbol)
        {
            return symbol switch
            {
                ModalTopSymbol.Gears => 1,
                _ => 1
            };
        }

        private void DrawScreenShadow(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_shadowTexture,
                new Rectangle(0, 0, _resolutionIndependentRenderer.VirtualWidth,
                    _resolutionIndependentRenderer.VirtualHeight),
                Color.White * 0.5f);
        }

        public void Show()
        {
            InitContent();
            IsVisible = true;
        }

        public void Update(GameTime gameTime, ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            // Poll for current keyboard state
            var state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape) || state.IsKeyDown(Keys.Space))
            {
                Close();
            }

            UpdateContent(gameTime, resolutionIndependenceRenderer);

            _closeButton.Update(resolutionIndependenceRenderer);
        }
    }
}