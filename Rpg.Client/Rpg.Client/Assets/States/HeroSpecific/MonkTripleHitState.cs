using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class MonkTripleHitState : IUnitStateEngine
    {
        private readonly CommonMeleeSkillUsageState _innerState;

        private bool _completionHandled;

        public MonkTripleHitState(UnitGraphics actorGraphics, UnitGraphicsBase targetGraphics,
            AnimationBlocker mainAnimationBlocker, SkillExecution interaction, IReadOnlyList<SoundEffectInstance> hitSounds)
        {
            var skillAnimationInfo = new SkillAnimationInfo
            {
                Items = new[]
                {
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 4,
                        HitSound = hitSounds[0],
                        Interaction = () => HandleInteractionByIndex(interaction, interactionIndex: 0),
                        InteractTime = 0
                    },
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 4,
                        HitSound = hitSounds[1],
                        Interaction = () => HandleInteractionByIndex(interaction, interactionIndex: 1),
                        InteractTime = 0
                    },
                    new SkillAnimationInfoItem
                    {
                        Duration = 1.75f / 2,
                        HitSound = hitSounds[2],
                        Interaction = () => HandleInteractionByIndex(interaction, interactionIndex: 2),
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
                PredefinedAnimationSid.Skill1);
        }

        private static void HandleInteractionByIndex(SkillExecution interaction, int interactionIndex)
        {
            var interactionSkillRuleInteraction = interaction.SkillRuleInteractions[interactionIndex];
            var targets = interactionSkillRuleInteraction.Targets;
            foreach (var target in targets)
            {
                interactionSkillRuleInteraction.Action(target);
            }
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            _innerState.Cancel();
        }

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