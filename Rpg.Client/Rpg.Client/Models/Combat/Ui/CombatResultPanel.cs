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

        public CombatResultPanel(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        public bool IsVisible { get; private set; }
        public string Result { get; set; }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var rect = new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2, PANEL_WIDTH, PANEL_HEIGHT);
            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result, rect.Location.ToVector2(), Color.Black);
            if (Result == "Win")
            {
                var benefitsExpVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2, 
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2 + 10);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Получили немного очков", benefitsExpVect, Color.Black);

                var benefitsLvlVect = new Vector2(benefitsExpVect.X, benefitsExpVect.Y + 10);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Получили уровень?", benefitsLvlVect, Color.Black);
            }
            else
            {
                var lostVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2, 
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2 + 10);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "К сожалению бой проигран", lostVect, Color.Brown);
            }
        }

        public void Initialize(string result)
        {
            Result = result;
            IsVisible = true;
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