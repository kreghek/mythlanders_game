using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class MonkTripleHitState : IUnitStateEngine
    {
        private readonly CommonMeleeSkillUsageState _innerState;

        public MonkTripleHitState(UnitGraphics actorGraphics, UnitGraphicsBase targetGraphics, AnimationBlocker mainAnimationBlocker, Action interaction, SoundEffectInstance hitSound)
        {
            var skillAnimationInfo = new SkillAnimationInfo
            {
                Items = new[]
                {
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 3,
                        HitSound = hitSound,
                        Interaction = interaction,
                        InteractTime = 0
                    },
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 3,
                        HitSound = hitSound,
                        Interaction = interaction,
                        InteractTime = 0
                    },
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 3,
                        HitSound = hitSound,
                        Interaction = interaction,
                        InteractTime = 0
                    }
                }
            };

            _innerState = new CommonMeleeSkillUsageState(
                actorGraphics,
                actorGraphics.Root,
                targetGraphics.Root,
                mainAnimationBlocker,
                skillAnimationInfo,
                AnimationSid.Skill1);
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }
        public void Cancel()
        {
            _innerState.Cancel();
        }

        private bool _completionHandled;

        public void Update(GameTime gameTime)
        {
            if (_innerState.IsComplete)
            {
                if (!_completionHandled)
                {
                    _completionHandled = true;
                    Completed?.Invoke(this, EventArgs.Empty);
                    IsComplete = true;
                }
            }

            _innerState.Update(gameTime);
        }

        public event EventHandler? Completed;
    }
}