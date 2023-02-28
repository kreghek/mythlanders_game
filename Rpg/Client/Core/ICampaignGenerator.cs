using System.Collections.Generic;

using Client.Core.Campaigns;

using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Core
{
    internal interface ICampaignGenerator
    {
        IReadOnlyList<HeroCampaign> CreateSet();
    }
}