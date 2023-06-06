using Client.Core;

namespace Client.Assets.StoryPointJobs;

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

    /// <summary>
    /// Complete any campaigns.
    /// </summary>
    public static readonly IJobType CompleteCampaigns = new JobType();
}