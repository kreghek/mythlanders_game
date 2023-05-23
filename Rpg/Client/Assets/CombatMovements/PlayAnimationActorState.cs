using System;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Play specified animation until it ends.
/// </summary>
/// <remarks>
/// To play animation longer just repeat animation multiple times.
/// </remarks>
internal sealed class PlayAnimationActorState : IActorVisualizationState
{
    private readonly IAnimationFrameSet _animation;
    private readonly IActorAnimator _animator;

    public PlayAnimationActorState(IActorAnimator animator, IAnimationFrameSet animation)
    {
        _animation = animation;
        _animator = animator;

        _animation.End += Animation_End;
    }

    private void Animation_End(object? sender, EventArgs e)
    {
        IsComplete = true;
    }

    /// <inheritdoc />
    public bool CanBeReplaced => false;

    /// <inheritdoc />
    public bool IsComplete { get; private set; }

    /// <inheritdoc />
    public void Cancel()
    {
        if (IsComplete)
        {
        }
    }

    private bool _animationStarted;

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (!_animationStarted)
        {
            _animator.PlayAnimation(_animation);
            _animationStarted = true;
        }
    }
}