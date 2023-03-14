using System.Collections.Generic;
using System.Linq;

using Core.Balance;

namespace BalanceConverter
{
    internal static class RowConverter
    {
        public static IReadOnlyCollection<BalanceTableRecord> Convert(IEnumerable<UnitExcelRow> unitExcelRows)
        {
            return unitExcelRows.Select(x => new BalanceTableRecord
            {
                Sid = x.Sid,
                TankRank = x.TankRank,
                DamageDealerRank = x.DamageDealerRank,
                SupportRank = x.SupportRank
            }).ToList();
        }

        public static CommonUnitBasics ConvertToUnitBasic(IEnumerable<UnitBasicRow> unitExcelRows)
        {
            var basics = new CommonUnitBasics();

            var properties = typeof(CommonUnitBasics).GetProperties();
            var unitBasicRowsArray = unitExcelRows as UnitBasicRow[] ?? unitExcelRows.ToArray();
            foreach (var property in properties)
            {
                var row = unitBasicRowsArray.Single(x => x.Key == property.Name);
                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(basics, (int)row.Value);
                }
                else
                {
                    property.SetValue(basics, row.Value);
                }
            }

            return basics;
        }
    }
}