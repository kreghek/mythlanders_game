using System.Collections.Generic;

using Client.Core.Campaigns;

namespace Rpg.Client.Core
{
    internal interface ICampaignGenerator
    {
        IReadOnlyList<HeroCampaign> CreateSet();
    }
}