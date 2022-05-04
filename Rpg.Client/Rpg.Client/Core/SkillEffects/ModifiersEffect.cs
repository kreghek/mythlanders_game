using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class ModifiersEffect : PeriodicEffectBase
    {
        public ModifiersEffect(ICombatUnit actor, int duration) : base(actor, duration)
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

            foreach (var modifier in Modifiers)
            {
                CombatContext.Combat.ModifiersProcessor.RegisterModifier(Target, modifier);
            }
        }
    }
}