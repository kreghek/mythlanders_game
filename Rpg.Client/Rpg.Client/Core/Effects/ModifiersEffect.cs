using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;

namespace Rpg.Client.Core.Effects
{
    internal abstract class ModifiersEffect : PeriodicEffectBase
    {
        public abstract IEnumerable<ModifierBase> Modifiers { get; }

        public ModifiersEffect()
        {
            Imposed += ModifiersEffect_Imposed;
            Dispelled += ModifiersEffect_Dispelled;
        }

        private void ModifiersEffect_Dispelled(object? sender, UnitEffectEventArgs e)
        {
            Imposed -= ModifiersEffect_Imposed;
            Dispelled -= ModifiersEffect_Dispelled;

            if (Target is null)
                return;

            if (Modifiers is null)
                return;

            foreach (var modifier in Modifiers)
                Combat.ModifiersProcessor.RemoveModifier(Target, modifier);
        }

        private void ModifiersEffect_Imposed(object? sender, UnitEffectEventArgs e)
        {
            if (Target is null)
                return;

            if (Modifiers is null)
                return;

            foreach (var modifier in Modifiers)
                Combat.ModifiersProcessor.RegisterModifier(Target, modifier);
        }
    }
}