using CombatDicesTeam.Combats;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal static class SkillEffectDrawerHelper
{
    internal static string GetLocalized(ITargetSelector direction)
    {
        return direction.ToString() ?? direction.GetType().ToString();
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