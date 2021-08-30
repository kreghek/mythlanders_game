using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class CombatResultPanel
    {
        private readonly IUiContentStorage _uiContentStorage;
        private string _result;

        public CombatResultPanel(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        public void Initialize(string result)
        {
            _result = result;
            IsVisible = true;
        }

        public bool IsVisible { get; private set; }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var rect = new Rectangle(graphicsDevice.Viewport.Bounds.X - 100, graphicsDevice.Viewport.Bounds.Y - 10, 200, 20);
            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _result, rect.Location.ToVector2(), Color.Black);
        }
    }
}
