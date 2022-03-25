using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class AnimatedCountableUnitItemStat
    {
        private const int MINIMAL_COUNTER_SPEED = 2;
        private const int MINIMAL_COUNTER_THRESHOLD = 100;

        private readonly int _counterSpeed;

        private int _countedValue;

        private bool _countingComplete;

        public AnimatedCountableUnitItemStat(ResourceReward item)
        {
            Amount = item.Amount;
            CurrentValue = item.StartValue;

            _counterSpeed = CalcCounterSpeed();
            Type = item.Type;
        }

        public int Amount { get; }

        public int CurrentValue { get; private set; }

        public EquipmentItemType Type { get; }

        public void Update()
        {
            if (_countingComplete)
            {
                return;
            }

            if (Amount == 0)
            {
                _countingComplete = true;
                return;
            }

            CurrentValue += _counterSpeed;
            _countedValue += _counterSpeed;

            if (_countedValue >= Amount)
            {
                _countingComplete = true;
            }
        }

        private int CalcCounterSpeed()
        {
            int counterSpeed;
            if (Math.Abs(Amount) > MINIMAL_COUNTER_THRESHOLD)
            {
                counterSpeed =
                    (int)Math.Max(
                        Math.Round((float)Amount / MINIMAL_COUNTER_THRESHOLD, MidpointRounding.AwayFromZero), 1);
            }
            else
            {
                counterSpeed = Amount switch
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
}