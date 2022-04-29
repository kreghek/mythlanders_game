using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States
{
    internal class CommonSupportSkillUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly UnitGraphics _graphics;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public CommonSupportSkillUsageState(UnitGraphics graphics, AnimationBlocker blocker, Action interaction,
            Microsoft.Xna.Framework.Audio.SoundEffectInstance hitSound, AnimationSid animationSid)
        {
            _graphics = graphics;
            _blocker = blocker;

            _subStates = new IUnitStateEngine[]
            {
                new HealState(_graphics, interaction, hitSound, animationSid)
            };
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            if (_subStateIndex < _subStates.Length)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
                _blocker.Release();
                Completed?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? Completed;
    }
}