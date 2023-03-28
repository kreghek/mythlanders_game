using Core.Combats;

namespace Client.Core;

/// <summary>
/// State of the hero during campaign
/// </summary>
internal sealed class HeroCampaignState
{
    public HeroCampaignState(string classSid, IStatValue hitPoints)
    {
        ClassSid = classSid;
        HitPoints = hitPoints;
    }

    /// <summary>
    /// Hero class identifier.
    /// </summary>
    public string ClassSid { get; }

    /// <summary>
    /// Current hero hit points.
    /// Keep hit points between combats and crises.
    /// </summary>
    public IStatValue HitPoints { get; }
}