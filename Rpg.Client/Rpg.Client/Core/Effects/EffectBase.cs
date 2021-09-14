using System;
using System.Diagnostics;

namespace Rpg.Client.Core.Effects
{
    internal abstract class EffectBase
    {
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
            Influenced?.Invoke(this, Target);
        }

        protected abstract void InfluenceAction();

        public event EventHandler<CombatUnit>? Imposed;
        public event EventHandler<CombatUnit>? Dispelled;
        public event EventHandler<CombatUnit>? Influenced;
    }
}