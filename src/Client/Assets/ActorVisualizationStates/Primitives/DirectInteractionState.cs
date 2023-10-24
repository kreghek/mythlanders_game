using System;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal sealed class DirectInteractionState : IActorVisualizationState
{
    private readonly IActorAnimator _actor;
    private readonly IAnimationFrameSet _animation;
    private readonly SkillAnimationInfo _animationInfo;
    private int _animationItemIndex;
    private bool _animationStarted;
    private double _counter;

    private bool _interactionExecuted;

    public DirectInteractionState(
        IActorAnimator actor,
        SkillAnimationInfo animationInfo,
        IAnimationFrameSet animation)
    {
        _animationInfo = animationInfo;
        _animation = animation;
        _actor = actor;
    }

    private static void HandleStateExecution(Action interaction, SoundEffectInstance? interactionSound)
    {
        interactionSound?.Play();

        interaction.Invoke();
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
    }

    public void Update(GameTime gameTime)
    {
        if (!_animationStarted)
        {
            _actor.PlayAnimation(_animation);

            _animationStarted = true;
        }

        _counter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_animationItemIndex <= _animationInfo!.Items!.Count - 1)
        {
            var currentAnimationItem = _animationInfo.Items[_animationItemIndex];

            if (_counter >= currentAnimationItem.Duration)
            {
                _interactionExecuted = false;
                _animationItemIndex++;
                _counter = 0;
            }
            else if (_counter >= currentAnimationItem.InteractTime)
            {
                if (!_interactionExecuted)
                {
                    HandleStateExecution(currentAnimationItem.Interaction!, currentAnimationItem.HitSound);
                    _interactionExecuted = true;
                }
            }
        }
        else
        {
            IsComplete = true;
        }
    }
}