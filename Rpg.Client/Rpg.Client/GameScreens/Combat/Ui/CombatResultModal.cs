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
        private readonly CombatRewards _combatItems;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUiContentStorage _uiContentStorage;
        private CombatItem? _combatItemsLocal;

        private double _iterationCounter;

        public CombatResultModal(IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            CombatResult combatResult,
            CombatRewards combatItems) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            _combatItems = combatItems;
            CombatResult = combatResult;
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
            _combatItemsLocal = new CombatItem
            {
                UnitItems = _combatItems.UnitRewards.Select(x => new UnitItem(x)).ToArray(),
                BiomeProgress = new UnitItemStat(_combatItems.BiomeProgress)
            };
        }

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            if (_combatItemsLocal is null)
            {
                throw new InvalidOperationException();
            }

            _iterationCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_iterationCounter >= 0.01)
            {
                _combatItemsLocal.Update();
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
            if (_combatItemsLocal is null)
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

            spriteBatch.DrawString(resultTitleFont, localizedCombatResultText, resultPosition,
                Color.Wheat);

            var benefitsPosition = new Vector2(contentRect.Location.X + MARGIN,
                resultPosition.Y + resultTitleSize.Y + MARGIN);
            var biomeProgress =
                $"{_combatItemsLocal.BiomeProgress.CurrentValue}/{_combatItemsLocal.BiomeProgress.LevelupSelector()} biome level";
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeProgress,
                benefitsPosition + new Vector2(32 + MARGIN + 100, benefitsPosition.Y),
                Color.Wheat);

            var xpItems = _combatItemsLocal.UnitItems.ToArray();
            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];
                var benefitsLvlPosition =
                    new Vector2(benefitsPosition.X, benefitsPosition.Y + (32 + MARGIN) * itemIndex);

                var portraitRect = UnsortedHelpers.GetUnitPortraitRect(item.UnitName);

                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), benefitsLvlPosition, portraitRect,
                    Color.White);

                var localizedName = GameObjectHelper.GetLocalized(item.UnitName);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedName,
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 0),
                    Color.Wheat);

                var unitXpBenefit = UnitValueBenefit(unitItemStat: item.Xp, "XP");

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitXpBenefit,
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 10 + MARGIN),
                    Color.Wheat);

                var unitEquipmentBenefit = UnitValueBenefit(unitItemStat: item.Equipment, "Equipment");

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitEquipmentBenefit,
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 20 + MARGIN),
                    Color.Wheat);
            }
        }

        private static string GetCombatResultLocalizedText(CombatResult combatResult)
        {
            return combatResult switch
            {
                CombatResult.Victory => UiResource.CombatResultVictoryText,
                CombatResult.Defeat => UiResource.CombatResultDefeatText,
                CombatResult.NextCombat => UiResource.CombatResultNextText,
                _ => throw new ArgumentOutOfRangeException(nameof(combatResult), combatResult, null)
            };
        }

        private static string? UnitValueBenefit(UnitItemStat? unitItemStat, string postfix)
        {
            var unitXpBenefit = $"{unitItemStat.CurrentValue}/{unitItemStat.LevelupSelector()} {postfix}";

            if (unitItemStat.IsShowLevelUpIndicator is not null)
            {
                unitXpBenefit += " LEVELUP!";

                if (unitItemStat.IsShowLevelUpIndicator > 1)
                {
                    unitXpBenefit += $" x {unitItemStat.IsShowLevelUpIndicator}";
                }
            }

            return unitXpBenefit;
        }

        private sealed class CombatItem
        {
            public UnitItemStat? BiomeProgress { get; init; }
            public IReadOnlyCollection<UnitItem>? UnitItems { get; init; }

            public void Update()
            {
                BiomeProgress?.Update();

                UpdateUnitItems();
            }

            private void UpdateUnitItems()
            {
                if (UnitItems is null)
                {
                    return;
                }

                foreach (var unitItem in UnitItems)
                {
                    unitItem.Update();
                }
            }
        }

        private sealed class UnitItemStat
        {
            const int MINIMAL_COUNTER_SPEED = 2;
            const int MINIMAL_COUNTER_THRESHOLD = 100;
            
            private readonly int _counterSpeed;

            public UnitItemStat(RewardStat item)
            {
                _amount = item.Amount;
                LevelupSelector = item.ValueToLevelupSelector;
                CurrentValue = item.StartValue;

                _counterSpeed = CalcCounterSpeed();
            }

            private int CalcCounterSpeed()
            {
                int counterSpeed;
                if (Math.Abs(_amount) > MINIMAL_COUNTER_THRESHOLD)
                {
                    counterSpeed =
                        (int)Math.Max(Math.Round((float)_amount / MINIMAL_COUNTER_THRESHOLD, MidpointRounding.AwayFromZero), 1);
                }
                else
                {
                    counterSpeed = _amount switch
                    {
                        > 0 => MINIMAL_COUNTER_SPEED,
                        < 0 => -MINIMAL_COUNTER_SPEED,
                        _ => throw new InvalidOperationException(
                            $"{nameof(_amount)} required to be greatest that zero.")
                    };
                }

                return counterSpeed;
            }

            private int _countedValue;

            public int CurrentValue { get; private set; }

            public int? IsShowLevelUpIndicator { get; private set; }

            private readonly int _amount;

            private bool _countingComplete;
            public Func<int> LevelupSelector { get; }

            public void Update()
            {
                if (_countingComplete)
                {
                    return;
                }

                if (_amount == 0)
                {
                    _countingComplete = true;
                    return;
                }

                CurrentValue += _counterSpeed;
                _countedValue += _counterSpeed;

                if (CurrentValue >= LevelupSelector())
                {
                    CurrentValue -= LevelupSelector();

                    if (IsShowLevelUpIndicator is null)
                    {
                        IsShowLevelUpIndicator = 1;
                    }
                    else
                    {
                        IsShowLevelUpIndicator++;
                    }
                }

                if (_countedValue >= _amount)
                {
                    _countingComplete = true;
                }
            }
        }

        private sealed class UnitItem
        {
            public UnitItem(UnitRewards rewards)
            {
                UnitName = rewards.Unit.UnitScheme.Name;
                Xp = new UnitItemStat(rewards.Xp);

                if (rewards.Equipment is not null)
                {
                    Equipment = new UnitItemStat(rewards.Equipment);
                }
            }

            public UnitItemStat? Equipment { get; }
            public UnitName UnitName { get; }

            public UnitItemStat? Xp { get; }

            public void Update()
            {
                Xp?.Update();
                Equipment?.Update();
            }
        }
    }
}