using GameClient.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

public sealed class DelayActorState : IActorVisualizationState
{
    private double _counterSeconds;

    public DelayActorState(Duration duration)
    {
        _counterSeconds = duration.Seconds;
    }

    public bool CanBeReplaced => true;

    public bool IsComplete => _counterSeconds <= 0;

    public void Cancel()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        _counterSeconds -= gameTime.ElapsedGameTime.TotalSeconds;
    }
}