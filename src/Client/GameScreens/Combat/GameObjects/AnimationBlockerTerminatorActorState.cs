using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

public class AnimationBlockerTerminatorActorState : IActorVisualizationState
{
    private readonly IAnimationBlocker _animationBlocker;

    public AnimationBlockerTerminatorActorState(IAnimationBlocker animationBlocker)
    {
        _animationBlocker = animationBlocker;
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        if (IsComplete)
        {
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        _animationBlocker.Release();
        IsComplete = true;
    }
}