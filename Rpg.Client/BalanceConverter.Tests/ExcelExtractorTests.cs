using System.IO;
using System.Reflection;

using BalanceConverter;

using FluentAssertions;

using NUnit.Framework;

namespace BalanceConverter.Tests
{
    public class ExcelExtractorTests
    {
        [Test]
        public void Test1()
        {
            // ACT

            var rows = BalanceConverter.ExcelExtractor.ReadUnitsFromExcel(BalanceConverter.ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");

            // ASSERT

            rows.Should().NotBeEmpty();
        }
    }
}