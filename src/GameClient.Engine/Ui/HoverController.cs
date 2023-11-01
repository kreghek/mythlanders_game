namespace GameClient.Engine.Ui;

/// <summary>
/// Class to control hover/leave events in UI-controls.
/// </summary>
/// <typeparam name="TPayload"> Payload type </typeparam>
public sealed class HoverController<TPayload>
{
    /// <summary>
    /// Current value under hover.
    /// </summary>
    public TPayload? CurrentValue { get; private set; }

    /// <summary>
    /// Drop <see cref="CurrentValue" /> to default value
    /// </summary>
    public void ForcedDrop()
    {
        CurrentValue = default;
    }

    /// <summary>
    /// Handles hover event.
    /// </summary>
    /// <param name="hoverValue">Value of element that was hovered.</param>
    public void HandleHover(TPayload? hoverValue)
    {
        if (EqualityComparer<TPayload>.Default.Equals(hoverValue, CurrentValue))
        {
            // Do not raise hover event again. Because value is not changed.
            return;
        }

        CurrentValue = hoverValue;
        Hover?.Invoke(this, hoverValue);
    }

    /// <summary>
    /// Handles leave event.
    /// </summary>
    /// <param name="hoverValue"> Value of element that was left. </param>
    public void HandleLeave(TPayload? hoverValue)
    {
        if (!EqualityComparer<TPayload>.Default.Equals(hoverValue, CurrentValue))
        {
            return;
        }

        CurrentValue = default;
        Leave?.Invoke(this, hoverValue);
    }

    /// <summary>
    /// Hover event.
    /// </summary>
    public event EventHandler<TPayload?>? Hover;
    
    /// <summary>
    /// Leave event.
    /// </summary>
    public event EventHandler<TPayload?>? Leave;
}