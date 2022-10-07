using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Core
{
    internal interface ICampaignGenerator
    {
        IReadOnlyList<HeroCampaign> CreateSet();
    }
}