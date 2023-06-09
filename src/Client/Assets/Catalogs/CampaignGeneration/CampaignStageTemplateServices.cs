﻿using Client.Core;

using Core.Dices;
using Core.PropDrop;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Common services to create stages from template.
/// </summary>
internal sealed record CampaignStageTemplateServices(
    IEventCatalog EventCatalog,
    GlobeProvider GlobeProvider,
    IJobProgressResolver JobProgressResolver,
    IDropResolver DropResolver,
    IDice Dice);