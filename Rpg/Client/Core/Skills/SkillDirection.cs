using Client.Core.Skills;

namespace Rpg.Client.Core.Skills
{
    internal static class SkillDirection
    {
        public static readonly ITargetSelector Target = new TargetTargetSelector();
        public static readonly ITargetSelector Self = new SelfTargetSelector();
        public static readonly ITargetSelector OtherFriendly = new OtherFriendlyTargetSelector();
        public static readonly ITargetSelector AllEnemies = new AllEnemiesTargetSelector();
        public static readonly ITargetSelector AllLineEnemies = new AllLineEnemiesTargetSelector();
        public static readonly ITargetSelector AllFriendly = new AllFriendlyTargetSelector();
        public static readonly ITargetSelector RandomEnemy = new RandomEnemyTargetSelector();
        public static readonly ITargetSelector RandomLineEnemy = new RandomLineEnemyTargetSelector();
        public static readonly ITargetSelector RandomFriendly = new RandomFriendlyTargetSelector();
        public static readonly ITargetSelector Other = new OtherTargetSelector();
        public static readonly ITargetSelector All = new AllTargetSelector();
    }
}