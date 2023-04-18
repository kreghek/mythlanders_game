using Client.Core;

using Core.Dices;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Common services to create stages from template.
/// </summary>
internal sealed record CampaignStageTemplateServices(
    IUnitSchemeCatalog UnitSchemeCatalog,
    IEventCatalog EventCatalog,
    GlobeProvider GlobeProvider,
    IDice Dice);