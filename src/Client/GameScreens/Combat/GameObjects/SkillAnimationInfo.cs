using System.Collections.Generic;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class SkillAnimationInfo
{
    public IReadOnlyList<SkillAnimationInfoItem> Items { get; set; }
}