using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class SkillEffectExecutionItem
    {
        public Action<ICombatUnit> Action { get; set; }
        public IReadOnlyList<ICombatUnit> Targets { get; set; }
        public ISkillEffectMetadata? Metadata { get; set; }
    }
}