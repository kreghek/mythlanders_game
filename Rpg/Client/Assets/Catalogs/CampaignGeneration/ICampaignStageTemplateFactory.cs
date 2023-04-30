using System.Collections.Generic;

using Client.Core.Campaigns;

using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Factory of campaign stage from a template.
/// </summary>
internal interface ICampaignStageTemplateFactory: IGraphTemplate<ICampaignStageItem>
{
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems);

    /// <summary>
    /// Create stage item of the campaign.
    /// </summary>
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems);
}