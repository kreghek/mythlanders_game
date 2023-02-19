using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Factory of campaign stage from a template.
/// </summary>
internal interface ICampaignStageTemplateFactory
{
    /// <summary>
    /// Create stage item of the campaign.
    /// </summary>
    /// <returns></returns>
    public ICampaignStageItem Create();

    public bool CanCreate();
}