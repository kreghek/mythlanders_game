using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal interface IEffectLifetime
    {
        void Update();

        event EventHandler? Disposed;

        string GetTextDescription();

        void MergeWith(IEffectLifetime effect);
    }

    internal sealed class DurationEffectLifetime : IEffectLifetime
    {
        public event EventHandler? Disposed;

        public string GetTextDescription()
        {
            return _duration.ToString();
        }

        public void Update()
        {
            _duration--;

            if (_duration <= 0)
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MergeWith(IEffectLifetime effect)
        {
            if (effect is DurationEffectLifetime durationEffectLifetime)
            {
                _duration += durationEffectLifetime._duration;
            }
        }

        private int _duration;

        public DurationEffectLifetime(int duration)
        {
            _duration = duration;
        }
    }

    internal abstract class PeriodicEffectBase : EffectBase
    {
        protected PeriodicEffectBase(ICombatUnit actor, IEffectLifetime effectLifetime)
        {
            Actor = actor;
            EffectLifetime = effectLifetime;
            effectLifetime.Disposed += EffectLifetime_Disposed;
        }

        private void EffectLifetime_Disposed(object? sender, EventArgs e)
        {
            if (sender is IEffectLifetime effectLifetime)
            {
                effectLifetime.Disposed -= EffectLifetime_Disposed;
            }
        }

        protected PeriodicEffectBase(ICombatUnit actor, int startDuration): this(actor, new DurationEffectLifetime(startDuration))
        {
        }

        protected PeriodicEffectBase(ICombatUnit actor) : this(actor, 1)
        {
        }

        public ICombatUnit Actor { get; }

        internal IEffectLifetime EffectLifetime { get; }

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

        protected override void InfluenceAction()
        {
            EffectLifetime.Update();
        }

        private bool CanBeMerged(PeriodicEffectBase testedEffect)
        {
            if (CombatContext is null)
            {
                throw new InvalidOperationException(
                    "CombatContext is not assigned. Use this method onl in combat environment.");
            }

            if (testedEffect.CombatContext is null)
            {
                throw new InvalidOperationException(
                    "CombatContext of tested effect is not assigned. Use this method onl in combat environment.");
            }

            var isSameType = testedEffect.GetType() != GetType();
            var isSameActor = Actor != testedEffect.Actor;
            var isSameSkill = CombatContext.EffectSource == testedEffect.CombatContext.EffectSource;

            return isSameType && isSameActor && isSameSkill;
        }

        private void MergeWithBase(PeriodicEffectBase targetEffect)
        {
            EffectLifetime.MergeWith(targetEffect.EffectLifetime);
        }
    }
}