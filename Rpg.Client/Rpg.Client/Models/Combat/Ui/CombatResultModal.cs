using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class CombatResultModal : ModalDialogBase
    {
        private readonly CombatResult _combatResult;
        private readonly IEnumerable<XpAward> _sourceXpItems;
        private readonly IUiContentStorage _uiContentStorage;

        private double _iterationCounter;

        private IEnumerable<XpItem> _xpItems;

        public CombatResultModal(IUiContentStorage uiContentStorage, GraphicsDevice graphicsDevice,
            CombatResult combatResult,
            IEnumerable<XpAward> xpItems) : base(uiContentStorage, graphicsDevice)
        {
            _uiContentStorage = uiContentStorage;
            _combatResult = combatResult;
            _sourceXpItems = xpItems;
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (_combatResult == CombatResult.Victory)
            {
                ShowVictoryBenefits(spriteBatch, ContentRect);
            }
            else if (_combatResult == CombatResult.NextCombat)
            {
                ShowVictoryBenefits(spriteBatch, ContentRect);
            }
            else if (_combatResult == CombatResult.Defeat)
            {
                ShowDefeatBenefits(spriteBatch, ContentRect);
            }
            else
            {
                Debug.Fail("Unknown combat result.");
            }
        }

        protected override void InitContent()
        {
            base.InitContent();
            _xpItems = _sourceXpItems.Select(x => new XpItem(x)).ToArray();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

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

        private void ShowDefeatBenefits(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var resultPosition = contentRect.Location.ToVector2() + new Vector2(5, 5);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _combatResult.ToString(), resultPosition,
                Color.Wheat);

            var biomeChangesPosition = resultPosition + new Vector2(0, 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Biome level: -50%", biomeChangesPosition,
                Color.Wheat);
        }

        private void ShowVictoryBenefits(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var xpItems = _xpItems.ToArray();

            var resultPosition = contentRect.Location.ToVector2() + new Vector2(5, 5);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _combatResult.ToString(), resultPosition,
                Color.Wheat);

            var benefitsPosition = resultPosition + new Vector2(0, 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Полученные улучшения:", benefitsPosition,
                Color.Wheat);

            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlVect = new Vector2(benefitsPosition.X, benefitsPosition.Y + 10 * (itemIndex + 1));
                var unitBenefit = $"{item.UnitName}: {item.CurrentXp}/{item.XpToLevelup} XP";

                if (item.IsShowLevelUpIndicator)
                {
                    unitBenefit += " LEVELUP!";
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitBenefit, benefitsLvlVect, Color.Wheat);
            }

            var biomeChangesPosition = benefitsPosition + new Vector2(0, 10) * xpItems.Length * 10;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Biome level: +1", biomeChangesPosition,
                Color.Wheat);
        }

        private sealed class XpItem
        {
            private const int XP_COUNTER_SPEED = 2;

            public XpItem(XpAward item)
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