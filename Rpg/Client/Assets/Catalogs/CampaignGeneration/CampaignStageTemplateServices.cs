using Core.Dices;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed record CampaignStageTemplateServices(IUnitSchemeCatalog UnitSchemeCatalog, IEventCatalog EventCatalog, GlobeProvider GlobeProvider, IDice Dice);
