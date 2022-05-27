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
                case SkillDirection.RandomEnemy:
                    return UiResource.SkillDirectionRandomEnemyText;
                case SkillDirection.RandomFriendly:
                    return UiResource.SkillDirectionRandomFriendlyText;
                case SkillDirection.AllLineEnemies:
                    return UiResource.SkillDirectionAllLineEnemiesText;
                case SkillDirection.OtherFriendly:
                    return UiResource.SkillDirectionOtherFriendlyText;
                case SkillDirection.RandomLineEnemy:
                    return UiResource.SkillDirectionRandomLineEnemyText;
                case SkillDirection.Other:
                    return UiResource.SkillDirectionOtherText;
                case SkillDirection.All:
                    return UiResource.SkillDirectionAllText;
                default:
                    throw new ArgumentException($"{direction} is not known direction value.");
            }
        }
    }
}