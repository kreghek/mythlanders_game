﻿using System.Collections.Generic;

using Client.Core.Campaigns;

namespace Client.Core;

internal interface ICampaignGenerator
{
    IReadOnlyList<HeroCampaign> CreateSet();
}