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
                    //TODO Move to the dict
                    return "[RANDOM_ENEMY]";
                case SkillDirection.RandomFriendly:
                    //TODO Move to the dict
                    return "[RANDOM_FRIENDLY]";
                case SkillDirection.AllLineEnemies:
                    //TODO Move to the dict
                    return "[ALL_TANKING_ENEMIES]";
                default:
                    throw new ArgumentException($"{direction} is not known direction value.");
            }
        }
    }
}