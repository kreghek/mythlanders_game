using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class CombatResultModal : ModalDialogBase
    {
        private readonly TextButton _closeButton;
        private readonly IEnumerable<XpAward> _sourceXpItems;
        private readonly IUiContentStorage _uiContentStorage;

        private double _iterationCounter;

        private IEnumerable<XpItem> _xpItems;

        public CombatResultModal(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            CombatResult combatResult,
            IEnumerable<XpAward> xpItems) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _uiContentStorage = uiContentStorage;
            CombatResult = combatResult;
            _sourceXpItems = xpItems;

            _closeButton = new TextButton("Close", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), Rectangle.Empty);
            _closeButton.OnClick += CloseButton_OnClick;
        }

        internal CombatResult CombatResult { get; }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (CombatResult == CombatResult.Victory)
            {
                DrawVictoryBenefits(spriteBatch, ContentRect);
            }
            else if (CombatResult == CombatResult.NextCombat)
            {
                DrawNextCombatBenefits(spriteBatch, ContentRect);
            }
            else if (CombatResult == CombatResult.Defeat)
            {
                DrawDefeatBenefits(spriteBatch, ContentRect);
            }
            else
            {
                Debug.Fail("Unknown combat result.");
            }

            _closeButton.Rect = new Rectangle(ContentRect.Center.X - 50, ContentRect.Bottom - 25, 100, 20);
            _closeButton.Draw(spriteBatch);
        }

        protected override void InitContent()
        {
            base.InitContent();
            _xpItems = _sourceXpItems.Select(x => new XpItem(x)).ToArray();
        }

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

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

            _closeButton.Update(resolutionIndependenceRenderer);
        }

        private void CloseButton_OnClick(object? sender, EventArgs e)
        {
            Close();
        }

        private void DrawDefeatBenefits(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var resultPosition = contentRect.Location.ToVector2() + new Vector2(5, 5);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), CombatResult.ToString(), resultPosition,
                Color.Wheat);

            var biomeChangesPosition = resultPosition + new Vector2(0, 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Biome level: -50%", biomeChangesPosition,
                Color.Wheat);
        }

        private void DrawNextCombatBenefits(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var resultPosition = contentRect.Location.ToVector2() + new Vector2(5, 5);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), CombatResult.ToString(), resultPosition,
                Color.Wheat);
        }

        private void DrawVictoryBenefits(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var xpItems = _xpItems.ToArray();

            var resultPosition = contentRect.Location.ToVector2() + new Vector2(5, 5);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), CombatResult.ToString(), resultPosition,
                Color.Wheat);

            var benefitsPosition = resultPosition + new Vector2(0, 10);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Полученные улучшения:", benefitsPosition,
                Color.Wheat);

            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlVect = new Vector2(benefitsPosition.X, benefitsPosition.Y + 10 * (itemIndex + 1));
                var unitBenefit = $"{item.UnitName}: {item.CurrentXp}/{item.XpToLevelup} XP";

                if (item.IsShowLevelUpIndicator is not null)
                {
                    unitBenefit += " LEVELUP!";

                    if (item.IsShowLevelUpIndicator > 1)
                    {
                        unitBenefit += $" x {item.IsShowLevelUpIndicator}";
                    }
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitBenefit, benefitsLvlVect, Color.Wheat);
            }

            var biomeChangesPosition = benefitsPosition + new Vector2(0, 10) * (xpItems.Length + 1);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Biome level: +1", biomeChangesPosition,
                Color.Wheat);
        }

        private sealed class XpItem
        {
            private const int XP_COUNTER_SPEED = 2;

            public XpItem(XpAward item)
            {
                var rm = new ResourceManager(typeof(UiResource));
                var name = rm.GetString(item.Unit.UnitScheme.Name.ToString()) ?? item.Unit.UnitScheme.Name.ToString();
                UnitName = name;
                XpAmount = item.XpAmount;
                StartXp = item.StartXp;
                XpToLevelup = item.XpToLevelup;
                CurrentXp = StartXp;
            }

            public int CountedXp { get; private set; }

            public int CurrentXp { get; private set; }

            public bool IsLevelUp => StartXp + XpAmount >= XpToLevelup;

            public int? IsShowLevelUpIndicator { get; private set; }
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
                    CurrentXp -= XpToLevelup;

                    if (IsShowLevelUpIndicator is null)
                    {
                        IsShowLevelUpIndicator = 1;
                    }
                    else
                    {
                        IsShowLevelUpIndicator++;
                    }
                }

                if (CountedXp >= XpAmount)
                {
                    XpCountingComplete = true;
                }
            }
        }
    }
}