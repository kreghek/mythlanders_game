using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    using System.Linq;

    using Core;

    internal sealed class CombatResultPanel
    {
        private const int PANEL_HEIGHT = 40;
        private const int PANEL_WIDTH = 400;
        private readonly IUiContentStorage _uiContentStorage;

        private ActiveCombat? _combat;

        public CombatResultPanel(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        public bool IsVisible { get; private set; }
        public string Result { get; set; }

        private GainLevelResult[]? _xpItems;

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (Result == "Win")
            {
                ShowWinBenefits(spriteBatch, graphicsDevice);
            }
            else
            {
                var rect = new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2, PANEL_WIDTH, PANEL_HEIGHT);
                spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result, rect.Location.ToVector2(), Color.Black);

                var lostVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2 + 10);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "К сожалению бой проигран", lostVect,
                    Color.Brown);
            }
        }

        public void Initialize(string result, ActiveCombat activeCombat,
            System.Collections.Generic.IEnumerable<GainLevelResult> xpItems)
        {
            _combat = activeCombat;
            Result = result;
            _xpItems = xpItems.ToArray();
            IsVisible = true;
        }

        internal void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ShowWinBenefits(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var xpItems = _xpItems;

            var rect = new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2 + 10 * xpItems.Length, PANEL_WIDTH,
                PANEL_HEIGHT);
            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
            var resultVect = rect.Location.ToVector2();
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result, resultVect, Color.Black);

            var benefitsVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                resultVect.Y + 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Полученные улучшения:", benefitsVect, Color.Black);

            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlVect = new Vector2(benefitsVect.X, benefitsVect.Y + 10 * (itemIndex + 1));
                var unitBenefit = $"{item.Unit.UnitScheme.Name}: +{item.XpAmount}XP";

                if (item.IsLevelUp)
                {
                    unitBenefit += " LEVELUP!";
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitBenefit, benefitsLvlVect, Color.Black);
            }
        }

        public event EventHandler Closed;
    }
}