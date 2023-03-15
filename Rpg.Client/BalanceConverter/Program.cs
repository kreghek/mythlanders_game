using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Core.Balance;

namespace BalanceConverter;

internal static class Program
{
    private static void Main(string[] args)
    {
        var excelUnitRows = ExcelExtractor.ReadUnitsRolesFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");
        var unitRows = RowConverter.Convert(excelUnitRows);

        var excelBasicRows = ExcelExtractor.ReadUnitsBasicsFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Basics");
        var basics = RowConverter.ConvertToUnitBasic(excelBasicRows);

        var balanceData = new BalanceData(basics, unitRows.ToArray());

        var serialized = JsonSerializer.Serialize(balanceData, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        var outputPath = args[0];

        // Run with argument which contains full path to Rpg.Client/Resources directory
        var outputRuFileName = Path.Combine(outputPath, "Balance.json");
        File.WriteAllLines(outputRuFileName, new[] { serialized });
    }
}