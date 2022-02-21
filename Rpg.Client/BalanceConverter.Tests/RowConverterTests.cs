using System.Linq;

using FluentAssertions;

using NUnit.Framework;

namespace BalanceConverter.Tests
{
    public class RowConverterTests
    {
        [Test]
        public void Convert_ReadRealExcelBalanceFile_RowListIsNotEmptyAndSumUnitRankEquals1()
        {
            // ARRANGE

            var excelRows = ExcelExtractor.ReadUnitsRolesFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");

            // ACT

            var rows = RowConverter.Convert(excelRows);

            // ASSERT

            rows.Should().NotBeEmpty();

            var sums = rows.Select(item => item.TankRank + item.DamageDealerRank + item.SupportRank);
            var notOneSums = sums.Where(x => (int)x != 1);
            notOneSums.Should().BeEmpty("Every unit must have sum rank equals 1");
        }
    }
}