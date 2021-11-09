using System.Collections.Generic;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class SkillAnimationInfo
    {
        public IReadOnlyList<SkillAnimationInfoItem> Items { get; set; }
    }
}