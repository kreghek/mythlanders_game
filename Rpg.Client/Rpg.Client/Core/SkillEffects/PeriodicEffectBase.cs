using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class PeriodicEffectBase : EffectBase
    {
        public ICombatUnit Actor { get; private set; }

        private int _value = -1;

        protected PeriodicEffectBase(ICombatUnit actor, int startDuration)
        {
            Actor = actor;
            Duration = startDuration;
        }

        protected PeriodicEffectBase(ICombatUnit actor) : this(actor, 1)
        {
        }

        /// <summary>
        /// Duration of effect in combat rounds.
        /// </summary>
        public int Duration
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;
                if (_value <= 0)
                {
                    Dispel();
                }
            }
        }

        public override void AddToList(IList<EffectBase> list)
        {
            foreach (var effect in list)
            {
                if (effect is not PeriodicEffectBase periodicEffect)
                {
                    continue;
                }

                var canBeMerged = CanBeMerged(periodicEffect);
                if (!canBeMerged)
                {
                    continue;
                }

                MergeWithBase(periodicEffect);
                return;
            }

            base.AddToList(list);
        }

        private bool CanBeMerged(PeriodicEffectBase testedEffect)
        {
            if (CombatContext is null)
            {
                throw new InvalidOperationException("CombatContext is not assigned. Use this method onl in combat environment.");
            }

            if (testedEffect.CombatContext is null)
            {
                throw new InvalidOperationException("CombatContext of tested effect is not assigned. Use this method onl in combat environment.");
            }

            var isSameType = testedEffect.GetType() != GetType();
            var isSameActor = Actor != testedEffect.Actor;
            var isSameSkill = CombatContext.SourceSkill == testedEffect.CombatContext.SourceSkill;

            return isSameType && isSameActor && isSameSkill;
        }

        private void MergeWithBase(PeriodicEffectBase targetEffect)
        {
            targetEffect.Duration += Duration;
        }

        protected override void InfluenceAction()
        {
            if (Duration < 0)
            {
                Debug.Fail($"{nameof(Duration)} is not defined.");
            }

            Duration--;
        }
    }
}