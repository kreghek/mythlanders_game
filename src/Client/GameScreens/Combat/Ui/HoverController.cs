using System;
using System.Collections.Generic;

namespace Client.GameScreens.Combat.Ui;

public sealed class HoverController<T>
{
    public T? CurrentValue { get; private set; }

    public void HandleHover(T? hoverValue)
    {
        if (!EqualityComparer<T>.Default.Equals(hoverValue, default))
        {

            if (CurrentValue is null && !EqualityComparer<T>.Default.Equals(hoverValue, CurrentValue))
            {
                CurrentValue = hoverValue;
                Hover?.Invoke(this, hoverValue);
            }
        }
    }

    public void HandleLeave(T? hoverValue)
    {
        if (EqualityComparer<T>.Default.Equals(hoverValue, CurrentValue))
        {
            CurrentValue = default;
            Leave?.Invoke(this, hoverValue);
        }
    }

    public void ForcedDrop()
    {
        CurrentValue = default;
    }

    public event EventHandler<T?>? Hover;
    public event EventHandler<T?>? Leave;
}