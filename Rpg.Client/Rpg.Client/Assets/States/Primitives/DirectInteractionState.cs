using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.Primitives
{
    internal sealed class DirectInteractionState : IUnitStateEngine
    {
        private readonly IAnimationFrameSet? _animation;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly SkillAnimationInfo _animationInfo;
        private readonly PredefinedAnimationSid? _animationSid;
        private readonly UnitGraphics _graphics;

        private int _animationItemIndex;
        private bool _animationStarted;
        private double _counter;

        private bool _interactionExecuted;

        public DirectInteractionState(UnitGraphics graphics,
            SkillAnimationInfo animationInfo, PredefinedAnimationSid animationSid)
            : this(graphics, default, animationInfo, animationSid)
        {
        }

        private DirectInteractionState(
            UnitGraphics graphics,
            AnimationBlocker? animationBlocker,
            SkillAnimationInfo animationInfo,
            PredefinedAnimationSid animationSid)
        {
            _animationBlocker = animationBlocker;
            _animationInfo = animationInfo;
            _animationSid = animationSid;
            _graphics = graphics;
        }

        public DirectInteractionState(
            UnitGraphics graphics,
            AnimationBlocker? animationBlocker,
            SkillAnimationInfo animationInfo,
            IAnimationFrameSet animation)
        {
            _animationBlocker = animationBlocker;
            _animationInfo = animationInfo;
            _animation = animation;
            _graphics = graphics;
        }

        private void HandleStateEnding()
        {
            _animationBlocker?.Release();
        }

        private static void HandleStateExecution(Action interaction, SoundEffectInstance interactionSound)
        {
            interactionSound.Play();

            interaction.Invoke();
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (_animationBlocker is not null)
            {
                _animationBlocker.Release();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!_animationStarted)
            {
                if (_animationSid is not null)
                {
                    _graphics.PlayAnimation(_animationSid.Value);
                }
                else if (_animation is not null)
                {
                    _graphics.PlayAnimation(_animation);
                }
                else
                {
                    _graphics.PlayAnimation(PredefinedAnimationSid.Idle);
                    Debug.Fail("Any animation must be defined in the constructor.");
                }

                _animationStarted = true;
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationItemIndex <= _animationInfo.Items.Count - 1)
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
                        HandleStateExecution(currentAnimationItem.Interaction, currentAnimationItem.HitSound);
                        _interactionExecuted = true;
                    }
                }
            }
            else
            {
                IsComplete = true;

                HandleStateEnding();
            }
        }
    }
}