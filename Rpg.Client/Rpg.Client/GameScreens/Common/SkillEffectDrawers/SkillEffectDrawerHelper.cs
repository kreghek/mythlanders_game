using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal static class SkillEffectDrawerHelper
    {
        internal static string GetLocalized(ITargetSelector direction)
        {
            return direction.GetDescription();
        }
    }
}