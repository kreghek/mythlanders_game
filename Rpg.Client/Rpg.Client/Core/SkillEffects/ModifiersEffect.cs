using System;
using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class ModifiersEffect : PeriodicEffectBase
    {
        protected ModifiersEffect(ICombatUnit actor, IEffectLifetime effectLifetime) : base(actor, effectLifetime)
        {
            Imposed += ModifiersEffect_Imposed;
            Dispelled += ModifiersEffect_Dispelled;
        }

        protected abstract IEnumerable<ModifierBase> Modifiers { get; }

        private void ModifiersEffect_Dispelled(object? sender, UnitEffectEventArgs e)
        {
            Imposed -= ModifiersEffect_Imposed;
            Dispelled -= ModifiersEffect_Dispelled;

            if (Target is null)
            {
                return;
            }

            if (Modifiers is null)
            {
                return;
            }

            if (CombatContext is null)
            {
                throw new InvalidOperationException("Combat context bust be assigned");
            }

            foreach (var modifier in Modifiers)
            {
                CombatContext.Combat.ModifiersProcessor.RemoveModifier(Target, modifier);
            }
        }

        private void ModifiersEffect_Imposed(object? sender, UnitEffectEventArgs e)
        {
            if (Target is null)
            {
                return;
            }

            if (Modifiers is null)
            {
                return;
            }

            if (CombatContext is null)
            {
                throw new InvalidOperationException("Combat context bust be assigned");
            }

            foreach (var modifier in Modifiers)
            {
                CombatContext.Combat.ModifiersProcessor.RegisterModifier(Target, modifier);
            }
        }
    }
}