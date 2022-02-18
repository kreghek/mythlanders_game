using System.Linq;

using BalanceConverter;

using FluentAssertions;

using NUnit.Framework;

namespace BalanceConverter.Tests
{
    public class RowConverterTests
    {
        [Test]
        public void Test1()
        {
            // ARRANGE

            var excelRows = ExcelExtractor.ReadUnitsBasicsFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");

            // ACT

            var rows = RowConverter.ConvertToUnitBasic(excelRows);

            // ASSERT

            rows.Should().NotBeEmpty();

            var sums = rows.Select(item => item.TankRank + item.DamageDealerRank + item.SupportRank);
            var notOneSums = sums.Where(x => x != 1f);
            notOneSums.Should().BeEmpty("Every unit must have sum rank equals 1");
        }
    }
}