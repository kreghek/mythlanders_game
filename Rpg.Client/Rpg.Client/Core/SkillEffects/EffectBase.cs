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
        public virtual IEnumerable<EffectRule>? ImposeRules => default;
        public virtual IEnumerable<EffectRule>? InfluenceRules => default;
        public Func<CombatEffectContext, bool>? ImposeCondition { get; init; }
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
                Debug.Assert(false, "Эффект не наложен");
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
            if (ImposeCondition?.Invoke(CombatContext!) == false)
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
                Debug.Assert(false, "Эффект не наложен");
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