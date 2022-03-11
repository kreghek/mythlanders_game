using System;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal static class SkillEffectDrawerHelper
    {
        internal static string GetLocalized(SkillDirection direction)
        {
            switch (direction)
            {
                case SkillDirection.Target:
                    return UiResource.SkillDirectionTargetText;
                case SkillDirection.Self:
                    return UiResource.SkillDirectionSelfText;
                case SkillDirection.AllEnemies:
                    return UiResource.SkillDirectionAllEnemiesText;
                case SkillDirection.AllFriendly:
                    return UiResource.SkillDirectionAllFriendlyText;
                default:
                    throw new ArgumentException($"{direction} is not known direction value.");
            }
        }
    }
}