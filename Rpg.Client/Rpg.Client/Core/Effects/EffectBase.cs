using System;
using System.Collections.Generic;
using System.Diagnostics;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal abstract class EffectBase
    {
        public ActiveCombat Combat { get; set; }
        public IDice Dice { get; set; }

        public virtual IEnumerable<EffectRule>? DispelRules => default;

        public EffectProcessor EffectProsessor { get; set; }
        public virtual IEnumerable<EffectRule>? ImposeRules => default;
        public virtual IEnumerable<EffectRule>? InfluenceRules => default;
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
            EffectProsessor.Impose(DispelRules, Target, null);
            Dispelled?.Invoke(this, new UnitEffectEventArgs { Unit = Target, Effect = this });
            AfterDispel();
        }

        protected virtual void AfterDispel()
        {
        }

        /// <summary>
        /// Наложение.
        /// </summary>
        /// <param name="target"></param>
        public void Impose(CombatUnit target)
        {
            Target = target;
            IsImposed = true;
            EffectProsessor.Impose(ImposeRules, Target, null);
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
            EffectProsessor.Impose(InfluenceRules, Target, null);
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
            public CombatUnit Unit { get; set; }
        }
    }
}