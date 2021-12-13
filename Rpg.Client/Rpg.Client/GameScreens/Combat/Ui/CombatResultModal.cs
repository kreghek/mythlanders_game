using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class CombatResultModal : ModalDialogBase
    {
        private readonly TextButton _closeButton;
        private readonly CombatSource _combatSource;
        private readonly IReadOnlyCollection<XpAward> _sourceXpItems;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private double _iterationCounter;

        private IReadOnlyCollection<XpItem>? _xpItems;

        public CombatResultModal(IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            CombatResult combatResult,
            IReadOnlyCollection<XpAward> xpItems,
            CombatSource combatSource) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            CombatResult = combatResult;
            _sourceXpItems = xpItems;
            _combatSource = combatSource;
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
            if (_xpItems is null)
            {
                // The modal is not initialized yet.
                return;
            }

            var localizedCombatResultText = GetCombatResultLocalizedText(CombatResult);
            var resultTitleFont = _uiContentStorage.GetTitlesFont();
            var resultTitleSize = resultTitleFont.MeasureString(localizedCombatResultText);

            const int MARGIN = 5;
            var resultPosition = new Vector2(contentRect.Center.X, contentRect.Top + MARGIN) -
                                 new Vector2(resultTitleSize.X / 2, 0);
            
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedCombatResultText, resultPosition,
                Color.Wheat);

            var benefitsPosition = resultPosition + new Vector2(0, resultTitleSize.Y + MARGIN);

            var xpItems = _xpItems.ToArray();
            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlPosition = new Vector2(benefitsPosition.X, benefitsPosition.Y + (32 + MARGIN) * (itemIndex + 1));

                var portraitRect = UnsortedHelpers.GetUnitPortraitRect(item.UnitName);
                
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), benefitsLvlPosition, portraitRect, Color.White);

                var localizedName = GameObjectHelper.GetLocalized(item.UnitName);
                var unitBenefit = $"{localizedName}: {item.CurrentXp}/{item.XpToLevelupSelector()} XP";

                if (item.IsShowLevelUpIndicator is not null)
                {
                    unitBenefit += " LEVELUP!";

                    if (item.IsShowLevelUpIndicator > 1)
                    {
                        unitBenefit += $" x {item.IsShowLevelUpIndicator}";
                    }
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitBenefit, 
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 0), 
                    Color.Wheat);
            }

            if (!_combatSource.IsTrainingOnly)
            {
                var biomeChangesPosition = benefitsPosition + new Vector2(0, 10) * (xpItems.Length + 1);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Biome level: +1", biomeChangesPosition,
                    Color.Wheat);
            }
        }

        private static string GetCombatResultLocalizedText(CombatResult combatResult)
        {
            return combatResult switch
            {
                CombatResult.Victory => UiResource.CombatResultVictoryText,
                CombatResult.Defeat => "Поражение!",
                CombatResult.NextCombat => "Следующий бой...",
                _ => throw new ArgumentOutOfRangeException(nameof(combatResult), combatResult, null)
            };
        }

        private sealed class XpItem
        {
            private int XP_COUNTER_SPEED = 2;

            public XpItem(XpAward item)
            {
                UnitName = item.Unit.UnitScheme.Name;
                XpAmount = item.XpAmount;
                StartXp = item.StartXp;
                XpToLevelupSelector = item.XpToLevelupSelector;
                CurrentXp = StartXp;

                XP_COUNTER_SPEED = XpAmount / 100;
            }

            public int CountedXp { get; private set; }

            public int CurrentXp { get; private set; }

            public int? IsShowLevelUpIndicator { get; private set; }
            public int StartXp { get; }

            public UnitName UnitName { get; }
            public int XpAmount { get; }

            public bool XpCountingComplete { get; private set; }
            public Func<int> XpToLevelupSelector { get; }

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

                if (CurrentXp >= XpToLevelupSelector())
                {
                    CurrentXp -= XpToLevelupSelector();

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