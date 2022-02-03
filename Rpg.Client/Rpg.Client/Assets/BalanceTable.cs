using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal class BalanceTable
    {
        public BalanceTable()
        { 

        }

        public BalanceTableRecord GetRecord(UnitName unitName)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class BalanceTableRecord
    {
        public float SupportRank { get; init; }

        public float TankRank { get; init; }

        public float DamageDealerRank { get; init; }
    }
}
