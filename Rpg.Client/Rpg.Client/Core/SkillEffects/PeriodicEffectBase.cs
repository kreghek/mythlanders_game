using System.Collections.Generic;
using System.Diagnostics;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class PeriodicEffectBase : EffectBase
    {
        private int _value = -1;

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
                var canBeMerged = CanBeMerged(effect);
                if (canBeMerged)
                {
                    MergeWithBase(effect);
                    return;
                }
            }

            base.AddToList(list);
        }

        public virtual bool CanBeMerged(EffectBase testedEffect)
        {
            return false;
        }

        public abstract void MergeWithBase(EffectBase testedEffect);

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