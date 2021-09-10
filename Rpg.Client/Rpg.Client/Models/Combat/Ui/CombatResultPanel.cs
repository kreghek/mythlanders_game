using System;
using System.Collections.Generic;
using System.Linq;

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

        private double _iterationCounter;

        private XpItem[]? _xpItems;

        public CombatResultPanel(IUiContentStorage uiContentStorage)
        {
            _uiContentStorage = uiContentStorage;
        }

        public bool IsVisible { get; private set; }
        public CombatResult Result { get; set; }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (Result == CombatResult.Victory)
            {
                ShowWinBenefits(spriteBatch, graphicsDevice);
            }
            else
            {
                var rect = new Rectangle(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2, PANEL_WIDTH, PANEL_HEIGHT);
                spriteBatch.Draw(_uiContentStorage.GetButtonTexture(), rect, Color.White);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result.ToString(), rect.Location.ToVector2(),
                    Color.Black);

                var lostVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                    graphicsDevice.Viewport.Bounds.Center.Y - PANEL_HEIGHT / 2 + 10);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "К сожалению бой проигран", lostVect,
                    Color.Brown);
            }
        }

        public void Initialize(CombatResult result, IEnumerable<GainLevelResult> xpItems)
        {
            Result = result;
            _xpItems = xpItems.Select(x => new XpItem(x)).ToArray();
            IsVisible = true;
        }

        internal void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }

            if (_xpItems is null)
            {
                throw new InvalidOperationException();
            }

            _iterationCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_iterationCounter >= 0.01)
            {
                foreach (var item in _xpItems)
                {
                    item.Update();
                }

                _iterationCounter = 0;
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
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), Result.ToString(), resultVect, Color.Black);

            var benefitsVect = new Vector2(graphicsDevice.Viewport.Bounds.Center.X - PANEL_WIDTH / 2,
                resultVect.Y + 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Полученные улучшения:", benefitsVect, Color.Black);

            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlVect = new Vector2(benefitsVect.X, benefitsVect.Y + 10 * (itemIndex + 1));
                var unitBenefit = $"{item.UnitName}: {item.CurrentXp}/{item.XpToLevelup} XP";

                if (item.IsShowLevelUpIndicator)
                {
                    unitBenefit += " LEVELUP!";
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitBenefit, benefitsLvlVect, Color.Black);
            }
        }

        public event EventHandler? Closed;

        private sealed class XpItem
        {
            private const int XP_COUNTER_SPEED = 2;

            public XpItem(GainLevelResult item)
            {
                UnitName = item.Unit.UnitScheme.Name;
                XpAmount = item.XpAmount;
                StartXp = item.StartXp;
                XpToLevelup = item.XpToLevelup;
                CurrentXp = StartXp;
            }

            public int CountedXp { get; private set; }

            public int CurrentXp { get; private set; }

            public bool IsLevelUp => StartXp + XpAmount >= XpToLevelup;

            public bool IsShowLevelUpIndicator { get; private set; }
            public int StartXp { get; }

            public string UnitName { get; }
            public int XpAmount { get; }

            public bool XpCountingComplete { get; private set; }
            public int XpToLevelup { get; }

            public void Update()
            {
                if (XpCountingComplete)
                {
                    return;
                }

                if (XpAmount == 0)
                {
                    XpCountingComplete = true;
                    return;
                }

                CurrentXp += XP_COUNTER_SPEED;
                CountedXp += XP_COUNTER_SPEED;

                if (CurrentXp >= XpToLevelup)
                {
                    CurrentXp -= XP_COUNTER_SPEED;
                    IsShowLevelUpIndicator = true;
                }

                if (CountedXp >= XpAmount)
                {
                    XpCountingComplete = true;
                }
            }
        }
    }
}