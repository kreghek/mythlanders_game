using Client.Core;

namespace Client.Assets.StoryPointJobs;

internal static class JobTypeCatalog
{
    /// <summary>
    /// Win combats with any results.
    /// </summary>
    public static readonly IJobType WinCombats = new JobType();

    /// <summary>
    /// Defeat any enemies.
    /// </summary>
    public static readonly IJobType Defeats = new JobType();

    /// <summary>
    /// Win any campaigns.
    /// </summary>
    public static readonly IJobType WinCampaigns = new JobType();
}