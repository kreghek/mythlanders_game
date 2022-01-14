using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class CombatResultModal : ModalDialogBase
    {
        private readonly TextButton _closeButton;
        private readonly CombatRewards _combatRewards;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUiContentStorage _uiContentStorage;
        private CombatItem? _combatItemsLocal;

        private double _iterationCounter;

        public CombatResultModal(IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            CombatResult combatResult,
            CombatRewards combatRewards) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            _combatRewards = combatRewards;
            CombatResult = combatResult;
            _closeButton = new TextButton("Close", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), Rectangle.Empty);
            _closeButton.OnClick += CloseButton_OnClick;
        }

        internal CombatResult CombatResult { get; }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            switch (CombatResult)
            {
                case CombatResult.Victory:
                    DrawVictoryBenefits(spriteBatch, ContentRect);
                    break;
                case CombatResult.NextCombat:
                    DrawNextCombatBenefits(spriteBatch, ContentRect);
                    break;
                case CombatResult.Defeat:
                    DrawDefeatBenefits(spriteBatch, ContentRect);
                    break;
                default:
                    Debug.Fail("Unknown combat result.");
                    break;
            }

            _closeButton.Rect = new Rectangle(ContentRect.Center.X - 50, ContentRect.Bottom - 25, 100, 20);
            _closeButton.Draw(spriteBatch);
        }

        protected override void InitContent()
        {
            base.InitContent();

            var biomeProgress = new AnimatedProgressionUnitItemStat(_combatRewards.BiomeProgress);
            var unitRewards = _combatRewards.InventoryRewards.Select(x => new AnimatedRewardItem(x)).ToArray();

            _combatItemsLocal = new CombatItem(biomeProgress, unitRewards);
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
            var titlePosition = new Vector2(contentRect.Center.X, contentRect.Top + MARGIN) -
                                new Vector2(resultTitleSize.X / 2, 0);

            spriteBatch.DrawString(resultTitleFont, localizedCombatResultText, titlePosition,
                Color.Wheat);

            var benefitsPosition = new Vector2(contentRect.Location.X + MARGIN,
                titlePosition.Y + resultTitleSize.Y + MARGIN);

            var biomeProgress =
                string.Format(UiResource.CombatResultMonsterDangerIncreasedTemplate,
                    _combatItemsLocal.BiomeProgress.CurrentValue);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeProgress,
                new Vector2(MARGIN + contentRect.Center.X, benefitsPosition.Y),
                Color.Wheat);

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.CombatResultItemsFoundLabel, benefitsPosition, Color.White);
            var xpItems = _combatItemsLocal.UnitItems.ToArray();
            for (var itemIndex = 0; itemIndex < xpItems.Length; itemIndex++)
            {
                var item = xpItems[itemIndex];

                var itemOffsetVector = new Vector2(0, (32 + MARGIN) * itemIndex) + new Vector2(0, 20);

                var benefitsLvlPosition = benefitsPosition + itemOffsetVector;

                var resourceIconRect = GetEquipmentSpriteRect(item.Equipment.Type);

                spriteBatch.Draw(_gameObjectContentStorage.GetEquipmentIcons(), benefitsLvlPosition, resourceIconRect,
                    Color.White);

                var localizedName = GameObjectHelper.GetLocalized(item.Equipment.Type);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedName,
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 0),
                    Color.Wheat);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"x {item.Equipment.CurrentValue}",
                    benefitsLvlPosition + new Vector2(32 + MARGIN, 10 + MARGIN),
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

        private static int GetEquipmentSpriteIndex(EquipmentItemType equipmentItemType)
        {
            return (int)equipmentItemType;
        }

        private static Rectangle GetEquipmentSpriteRect(EquipmentItemType equipmentItemType)
        {
            const int COLUMN_COUNT = 2;
            const int ICON_SIZE = 32;

            var index = GetEquipmentSpriteIndex(equipmentItemType);

            var x = index % COLUMN_COUNT;
            var y = index / COLUMN_COUNT;

            return new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);
        }

        private sealed class CombatItem
        {
            public CombatItem(AnimatedProgressionUnitItemStat biomeProgress,
                IReadOnlyCollection<AnimatedRewardItem> unitItems)
            {
                BiomeProgress = biomeProgress;
                UnitItems = unitItems;
            }

            public AnimatedProgressionUnitItemStat BiomeProgress { get; }
            public IReadOnlyCollection<AnimatedRewardItem> UnitItems { get; }

            public void Update()
            {
                BiomeProgress?.Update();

                UpdateUnitItems();
            }

            private void UpdateUnitItems()
            {
                foreach (var unitItem in UnitItems)
                {
                    unitItem.Update();
                }
            }
        }

        private sealed class AnimatedProgressionUnitItemStat
        {
            private const int MINIMAL_COUNTER_SPEED = 2;
            private const int MINIMAL_COUNTER_THRESHOLD = 100;

            private readonly int _amount;

            private readonly int _counterSpeed;

            private int _countedValue;

            private bool _countingComplete;

            public AnimatedProgressionUnitItemStat(ProgressionRewardStat item)
            {
                _amount = item.Amount;
                LevelupSelector = item.ValueToLevelupSelector;
                CurrentValue = item.StartValue;

                _counterSpeed = CalcCounterSpeed();
            }

            public int CurrentValue { get; private set; }

            public int? IsShowLevelUpIndicator { get; private set; }
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

            private int CalcCounterSpeed()
            {
                int counterSpeed;
                if (Math.Abs(_amount) > MINIMAL_COUNTER_THRESHOLD)
                {
                    counterSpeed =
                        (int)Math.Max(
                            Math.Round((float)_amount / MINIMAL_COUNTER_THRESHOLD, MidpointRounding.AwayFromZero), 1);
                }
                else
                {
                    counterSpeed = _amount switch
                    {
                        > 0 => MINIMAL_COUNTER_SPEED,
                        < 0 => -MINIMAL_COUNTER_SPEED,
                        _ => MINIMAL_COUNTER_SPEED /*throw new InvalidOperationException(
                            $"{nameof(_amount)} required to be greatest that zero.")*/
                    };
                }

                return counterSpeed;
            }
        }

        private sealed class AnimatedCountableUnitItemStat
        {
            private const int MINIMAL_COUNTER_SPEED = 2;
            private const int MINIMAL_COUNTER_THRESHOLD = 100;

            private readonly int _amount;

            private readonly int _counterSpeed;

            private int _countedValue;

            private bool _countingComplete;

            public AnimatedCountableUnitItemStat(CountableRewardStat item)
            {
                _amount = item.Amount;
                CurrentValue = item.StartValue;

                _counterSpeed = CalcCounterSpeed();
                Type = item.Type;
            }

            public int CurrentValue { get; private set; }

            public EquipmentItemType Type { get; }

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

                if (_countedValue >= _amount)
                {
                    _countingComplete = true;
                }
            }

            private int CalcCounterSpeed()
            {
                int counterSpeed;
                if (Math.Abs(_amount) > MINIMAL_COUNTER_THRESHOLD)
                {
                    counterSpeed =
                        (int)Math.Max(
                            Math.Round((float)_amount / MINIMAL_COUNTER_THRESHOLD, MidpointRounding.AwayFromZero), 1);
                }
                else
                {
                    counterSpeed = _amount switch
                    {
                        > 0 => MINIMAL_COUNTER_SPEED,
                        < 0 => -MINIMAL_COUNTER_SPEED,
                        _ => MINIMAL_COUNTER_SPEED /*throw new InvalidOperationException(
                            $"{nameof(_amount)} required to be greatest that zero.")*/
                    };
                }

                return counterSpeed;
            }
        }

        private sealed class AnimatedRewardItem
        {
            public AnimatedRewardItem(CombatRewardsItem rewardItem)
            {
                Equipment = new AnimatedCountableUnitItemStat(rewardItem.Xp);
            }

            public AnimatedCountableUnitItemStat? Equipment { get; }

            public void Update()
            {
                Equipment?.Update();
            }
        }
    }
}