using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class SkillSet
    {
        public IReadOnlyList<SkillBase> Skills { get; set; }
    }
}