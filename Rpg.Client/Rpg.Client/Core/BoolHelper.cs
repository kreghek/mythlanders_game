namespace Rpg.Client.Core
{
    internal static class BoolHelper
    {
        public static bool IsAvailable(bool mainCondition, bool key)
        {
            return !mainCondition || (mainCondition && key);
        }

        public static bool HasNotRestriction(bool mainCondition, bool key)
        {
            return mainCondition || (!mainCondition && !key);
        }
    }
}