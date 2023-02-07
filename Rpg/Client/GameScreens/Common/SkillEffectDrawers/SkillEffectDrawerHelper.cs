using Client.Core.Skills;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal static class SkillEffectDrawerHelper
    {
        internal static string GetLocalized(ITargetSelector direction)
        {
            return direction.GetDescription();
        }

        internal static string GetPercent(float modifier)
        {
            if (modifier * 100 % 1 == 0)
            {
                return $"{modifier * 100:F0}";
            }

            return $"{modifier * 100:F1}";
        }
    }
}