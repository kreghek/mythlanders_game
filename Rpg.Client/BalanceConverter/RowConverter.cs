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
        
        public static UnitBasics ConvertToUnitBasic(IEnumerable<UnitBasicRow> unitExcelRows)
        {
            return new UnitBasics
            {
                ARMOR_BASE = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.ARMOR_BASE)).Value,
                POWER_BASE = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.POWER_BASE)).Value,
                DAMAGE_BASE = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.DAMAGE_BASE)).Value,
                SUPPORT_BASE = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.SUPPORT_BASE)).Value,
                HITPOINTS_BASE = (int)unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.HITPOINTS_BASE)).Value,
                POWER_PER_LEVEL_BASE = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.POWER_PER_LEVEL_BASE)).Value,
                HITPOINTS_PER_LEVEL_BASE = (int)unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.HITPOINTS_PER_LEVEL_BASE)).Value,
                HERO_POWER_MULTIPLICATOR = unitExcelRows.Single(x=>x.Key == nameof(UnitBasics.HERO_POWER_MULTIPLICATOR)).Value
            };
        }
    }
}