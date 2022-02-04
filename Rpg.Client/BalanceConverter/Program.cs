using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace BalanceConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var excelUnitRows = ExcelExtractor.ReadUnitsFromExcel(ExcelExtractor.SOURCE_EVENTS_EXCEL, "Units");
            var unitRows = RowConverter.Convert(excelUnitRows);

            var serialized = JsonSerializer.Serialize(unitRows, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                IgnoreNullValues = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });

            var outputPath = args[0];

            // Run with argument which contains full path to Rpg.Client/Resources directory

            var outputRuFileName = Path.Combine(outputPath, "Balance.json");
            File.WriteAllLines(outputRuFileName, new[] { serialized });
        }
    }
}