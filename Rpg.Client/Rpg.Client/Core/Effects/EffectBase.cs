using System;
using System.Collections.Generic;
using System.Diagnostics;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal abstract class EffectBase
    {
        public IDice Dice { get; set; }

        public abstract IEnumerable<EffectRule> DispelRules { get; }

        public EffectProcessor EffectProsessor { get; set; }
        public abstract IEnumerable<EffectRule> ImposeRules { get; }
        public abstract IEnumerable<EffectRule> InfluenceRules { get; }
        public CombatUnit? Target { get; private set; }
        protected bool IsImposed { get; private set; }

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
            EffectProsessor.Influence(DispelRules, Target, null);
            Dispelled?.Invoke(this, Target);
        }

        /// <summary>
        /// Наложение.
        /// </summary>
        /// <param name="target"></param>
        public void Impose(CombatUnit target)
        {
            Target = target;
            IsImposed = true;
            EffectProsessor.Influence(ImposeRules, Target, null);
            Imposed?.Invoke(this, Target);
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

            InfluenceAction();
            EffectProsessor.Influence(InfluenceRules, Target, null);
            Influenced?.Invoke(this, Target);
        }


        protected abstract void InfluenceAction();

        public event EventHandler<CombatUnit>? Imposed;
        public event EventHandler<CombatUnit>? Dispelled;
        public event EventHandler<CombatUnit>? Influenced;
    }
}