using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class AnimatedCountableUnitItemStat
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

        public int Amount => _amount;

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
}