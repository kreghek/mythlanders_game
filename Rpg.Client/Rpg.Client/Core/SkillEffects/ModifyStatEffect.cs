using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.SkillEffects
{
    internal abstract class ModifyStatEffectBase : PeriodicEffectBase
    {
        protected ModifyStatEffectBase(ICombatUnit actor, IEffectLifetime lifetime) : base(actor, lifetime)
        {
            Imposed += ModifiersEffect_Imposed;
            Dispelled += ModifiersEffect_Dispelled;
        }

        protected abstract IEnumerable<(UnitStatType, StatModifier)> Modifiers { get; }

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
                var stat = Target.Unit.Stats.Single(x => x.Type == modifier.Item1);
                stat.Value.RemoveModifier(modifier.Item2);
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
                var stat = Target.Unit.Stats.Single(x => x.Type == modifier.Item1);
                stat.Value.AddModifier(modifier.Item2);
            }
        }
    }
}
