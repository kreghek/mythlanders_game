using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;

namespace BalanceConverter
{
    internal static class RowConverter
    {
        public static IReadOnlyCollection<UnitRow> Convert(IEnumerable<UnitExcelRow> unitExcelRows)
        {
            return unitExcelRows.Select(x => new UnitRow
            {
                Sid = Enum.Parse<UnitName>(x.Sid),
                Type = string.Equals(x.Type, "hero", StringComparison.InvariantCultureIgnoreCase),
                Demo = string.Equals(x.Type, "demo", StringComparison.InvariantCultureIgnoreCase),
                TankRank = x.TankRank,
                DamageDealerRank = x.DamageDealerRank,
                SupportRank = x.SupportRank
            }).ToList();
        }
    }
}