using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class CombatResultPanel
    {
        private const int PANEL_HEIGHT = 40;
        private const int PANEL_WIDTH = 400;
        private readonly IUiContentStorage _uiContentStorage;
        private string _result;

        public CombatResultPanel(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        public void Initialize(string result)
        {
            Result = result;
            IsVisible = true;
        }

        public bool IsVisible { get; private set; }
        public string Result { get => _result; set => _result = value; }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var rect = new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH/2, graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2, PANEL_WIDTH, PANEL_HEIGHT);
            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result, rect.Location.ToVector2(), Color.Black);
        }

        internal void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler Closed;
    }
}
