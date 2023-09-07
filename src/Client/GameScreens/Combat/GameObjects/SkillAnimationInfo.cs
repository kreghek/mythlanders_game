using System.Collections.Generic;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class SkillAnimationInfo
{
    public IReadOnlyList<SkillAnimationStage> Items { get; set; }
}