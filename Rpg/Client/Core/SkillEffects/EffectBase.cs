using System;
using System.Collections.Generic;
using System.Diagnostics;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class EffectBase
    {
        public CombatEffectContext? CombatContext { get; set; }
        public virtual IEnumerable<EffectRule>? DispelRules => default;
        public IReadOnlyCollection<IEffectCondition>? ImposeConditions { get; init; }
        public virtual IEnumerable<EffectRule>? ImposeRules => default;
        public virtual IEnumerable<EffectRule>? InfluenceRules => default;
        public ICombatUnit? Target { get; private set; }
        public IEffectVisualization? Visualization { get; set; }
        protected bool IsImposed { get; private set; }

        public virtual void AddToList(IList<EffectBase> list)
        {
            list.Add(this);
        }

        /// <summary>
        /// Снятие.
        /// </summary>
        public void Dispel()
        {
            if (!IsImposed || Target is null)
            {
                Debug.Assert(false, "Effect is not imposed");
                return;
            }

            IsImposed = false;
            CombatContext.Combat.EffectProcessor.Impose(DispelRules, Target, null, CombatContext.EffectSource);
            Dispelled?.Invoke(this, new UnitEffectEventArgs { Unit = Target, Effect = this });
            AfterDispel();
        }

        /// <summary>
        /// Наложение.
        /// </summary>
        /// <param name="target"></param>
        public void Impose(ICombatUnit target)
        {
            var conditionPassed = CheckConditions(target);
            if (!conditionPassed)
            {
                // Do not impose effects failed the condition.
                return;
            }

            Target = target;
            IsImposed = true;
            CombatContext.Combat.EffectProcessor.Impose(ImposeRules, Target, null, CombatContext.EffectSource);
            Imposed?.Invoke(this, new UnitEffectEventArgs { Unit = Target, Effect = this });
            AfterImpose();
        }

        /// <summary>
        /// Воздействие.
        /// </summary>
        public void Influence()
        {
            if (!IsImposed || Target is null)
            {
                Debug.Assert(false, "Effect is not imposed");
                return;
            }

            Influenced?.Invoke(this, new UnitEffectEventArgs { Unit = Target, Effect = this });
            InfluenceAction();

            CombatContext.Combat.EffectProcessor.Impose(InfluenceRules, actor: Target, target: null,
                effectSource: CombatContext.EffectSource);
        }

        protected virtual void AfterDispel()
        {
        }

        protected virtual void AfterImpose()
        {
        }


        protected abstract void InfluenceAction();

        private bool CheckConditions(ICombatUnit target)
        {
            if (ImposeConditions is null)
            {
                return true;
            }

            if (CombatContext is null)
            {
                throw new InvalidOperationException();
            }

            foreach (var condition in ImposeConditions)
            {
                var conditionPassed = condition.Check(target, CombatContext);
                if (!conditionPassed)
                {
                    return false;
                }
            }

            return true;
        }

        public event EventHandler<UnitEffectEventArgs>? Imposed;
        public event EventHandler<UnitEffectEventArgs>? Dispelled;
        public event EventHandler<UnitEffectEventArgs>? Influenced;

        internal class UnitEffectEventArgs : EventArgs
        {
            public EffectBase Effect { get; set; }
            public ICombatUnit Unit { get; set; }
        }
    }
}