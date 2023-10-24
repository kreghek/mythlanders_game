using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

internal sealed class DefenseState : IActorVisualizationState
{
    private readonly UnitGraphics _graphics;
    private double _counter;

    public DefenseState(UnitGraphics graphics)
    {
        _graphics = graphics;
    }

    public bool CanBeReplaced => true;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        IsComplete = true;
    }

    public void Update(GameTime gameTime)
    {
        if (_counter == 0)
        {
            _graphics.PlayAnimation(PredefinedAnimationSid.Defense);
            _graphics.IsDamaged = true;
        }

        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_counter > 0.05)
        {
            _graphics.IsDamaged = false;
        }

        if (_counter > 1)
        {
            _graphics.IsDamaged = false;
            IsComplete = true;
        }
    }
}