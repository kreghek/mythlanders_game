using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPointJobs
{
    internal static class JobTypeCatalog
    {
        /// <summary>
        /// Complete combats with any results.
        /// </summary>
        public static readonly IJobType Combats = new JobType();

        /// <summary>
        /// Defeat any enemies.
        /// </summary>
        public static readonly IJobType Defeats = new JobType();
    }
}