using FluentAssertions;

using NUnit.Framework;

namespace BalanceConverter.Tests
{
    public class ExcelExtractorTests
    {
        [Test]
        public void ReadUnitsBasicsFromExcel_ExcelHasRows_ReturnsNotEmptyList()
        {
            // ACT

            var rows = ExcelExtractor.ReadUnitsBasicsFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Basics");

            // ASSERT

            rows.Should().NotBeEmpty();
        }

        [Test]
        public void ReadUnitsRolesFromExcel_ExcelHasRows_ReturnsNotEmptyList()
        {
            // ACT

            var rows = ExcelExtractor.ReadUnitsRolesFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");

            // ASSERT

            rows.Should().NotBeEmpty();
        }
    }
}