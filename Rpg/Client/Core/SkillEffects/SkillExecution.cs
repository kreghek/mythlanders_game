using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class SkillExecution
    {
        public Action SkillComplete { get; set; }
        public IReadOnlyList<SkillEffectExecutionItem> SkillRuleInteractions { get; set; }
    }
}