using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private readonly AnimationBlocker? _animationBlocker;
        private readonly SkillAnimationInfo _animationInfo;
        private readonly UnitGraphics _graphics;
        private readonly int _index;

        private int _animationItemIndex;
        private bool _animationStarted;
        private double _counter;

        private bool _interactionExecuted;

        public HitState(UnitGraphics graphics,
            SkillAnimationInfo animationInfo, int index)
            : this(graphics, default, animationInfo, index)
        {
        }

        public HitState(
            UnitGraphics graphics,
            AnimationBlocker? animationBlocker,
            SkillAnimationInfo animationInfo,
            int index)
        {
            _index = index;
            _animationBlocker = animationBlocker;
            _animationInfo = animationInfo;
            _graphics = graphics;
        }

        private void HandleStateEnding()
        {
            if (_animationBlocker is not null)
            {
                _animationBlocker.Release();
            }
        }

        private static void HandleStateExecution(Action interaction, SoundEffectInstance interactionSound)
        {
            interactionSound.Play();

            interaction?.Invoke();
        }

        public bool CanBeReplaced { get; }
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
                var skillText = $"Skill{_index}";
                var sid = Enum.Parse<AnimationSid>(skillText);
                _graphics.PlayAnimation(sid);

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