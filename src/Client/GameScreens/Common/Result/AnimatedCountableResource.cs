using System;

using Client.Core;
using Client.GameScreens.Combat;

namespace Client.GameScreens.Common.Result;

internal sealed class AnimatedCountableResource
{
    private const int MINIMAL_COUNTER_SPEED = 2;
    private const int MINIMAL_COUNTER_THRESHOLD = 100;

    private readonly int _counterSpeed;

    private int _countedValue;

    public AnimatedCountableResource(ResourceReward item)
    {
        Amount = item.Amount;
        CurrentValue = item.StartValue;

        _counterSpeed = CalcCounterSpeed();
        Type = item.Type;
    }

    public int Amount { get; }

    public int CurrentValue { get; private set; }

    public bool IsComplete { get; private set; }

    public EquipmentItemType Type { get; }

    public void Update()
    {
        if (IsComplete)
        {
            return;
        }

        if (Amount == 0)
        {
            IsComplete = true;
            return;
        }

        CurrentValue += _counterSpeed;
        _countedValue += _counterSpeed;

        if (_countedValue >= Amount)
        {
            IsComplete = true;
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
            var counterSpeedValue = Math.Min(MINIMAL_COUNTER_SPEED, Math.Abs(Amount));
            counterSpeed = Amount switch
            {
                > 0 => counterSpeedValue,
                < 0 => -counterSpeedValue,
                _ => counterSpeedValue /*throw new InvalidOperationException(
                        $"{nameof(_amount)} required to be greatest that zero.")*/
            };
        }

        return counterSpeed;
    }
}