using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPoints
{
    /// <summary>
    /// Область действия работы.
    /// </summary>
    internal static class JobScopeCatalog
    {
        /// <summary>
        /// Общая область действия.
        /// Прогресс не будет сбрасываться после окончания боя.
        /// Не влияет на одноходовые задачи (одним ударом убить 2 противника)
        /// </summary>
        public static readonly IJobScope Global = new JobScope();

        /// <summary>
        /// Область действия на высадку.
        /// Если работы в рамках одной высадки не выполнены, то прогресс будет сброшен.
        /// </summary>
        public static IJobScope Combat = new JobScope();
    }
}