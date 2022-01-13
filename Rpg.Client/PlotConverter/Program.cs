using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using ExcelDataReader;

namespace PlotConverter
{
    internal class Program
    {
        private static EventNodeDto BuildEventNodes(List<ExcelTextFragmentRow> excelTextFragments)
        {
            var fragments = new List<EventNodeTextFragment>();

            foreach (var excelTextFragment in excelTextFragments)
            {
                var fragment = new EventNodeTextFragment
                {
                    Speaker = excelTextFragment.Speaker,
                    Text = excelTextFragment.Text
                };
                fragments.Add(fragment);
            }

            var node = new EventNodeDto
            {
                Fragments = fragments.ToArray()
            };

            return node;
        }

        private static List<EventDto> ConventExcelRowsToObjectGraph(IReadOnlyCollection<ExcelEventRow> excelEventRows,
            IEnumerable<ExcelTextFragmentRow> excelRows)
        {
            var eventGrouped = excelRows.GroupBy(x => x.EventSid);

            var eventDtoList = new List<EventDto>();
            foreach (var excelEventGroup in eventGrouped)
            {
                var excelEvent = excelEventRows.Single(x => x.Sid == excelEventGroup.Key);

                var eventDto = new EventDto
                {
                    Sid = excelEvent.Sid,
                    Name = excelEvent.Name,
                    Location = excelEvent.Location,
                    BeforeCombatAftermath = excelEvent.BeforeCombatAftermath,
                    AfterCombatAftermath = excelEvent.AfterCombatAftermath,
                    GoalDescription = excelEvent.GoalDescription,
                    ParentSids = ParseParentSids(excelEvent)
                };
                eventDtoList.Add(eventDto);

                var excelBeforeEventNodes = excelEventGroup.Where(x => x.CombatPosition == -1).ToList();
                var beforeNode = BuildEventNodes(excelBeforeEventNodes);
                eventDto.BeforeCombatNode = beforeNode;

                var excelAfterEventNodes = excelEventGroup.Where(x => x.CombatPosition == 1).ToList();
                var afterNode = BuildEventNodes(excelAfterEventNodes);
                eventDto.AfterCombatNode = afterNode;
            }

            return eventDtoList;
        }

        private static string[] ParseParentSids(ExcelEventRow excelEvent)
        {
            if (excelEvent.ParentSids is null)
            {
                return null;
            }

            var sids = excelEvent.ParentSids
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return sids;
        }

        private static void Main(string[] args)
        {
            var outputPath = args[0];

            const string SOURCE_EVENTS_EXCEL = "Ewar - Plot.xlsx";
            var excelEventRows = ReadEventsFromExcel(SOURCE_EVENTS_EXCEL);
            var excelTextFragmentsRows = ReadTextFragmentsFromExcel(SOURCE_EVENTS_EXCEL);

            var eventDtoList = ConventExcelRowsToObjectGraph(excelEventRows, excelTextFragmentsRows);

            var serialized = JsonSerializer.Serialize(eventDtoList, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                IgnoreNullValues = true
            });

            // Run with argument which contains full path to Rpg.Client/Resources directory

            var outputRuFileName = Path.Combine(outputPath, "Plot-ru.txt");
            File.WriteAllLines(outputRuFileName, new[] { serialized });

            var outputEnFileName = Path.Combine(outputPath, "Plot-en.txt");
            File.WriteAllLines(outputEnFileName, new[] { serialized });
        }

        private static List<ExcelEventRow> ReadEventsFromExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelEventRow>();
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();

            var plotTable = result.Tables["Events"];

            // ReSharper disable once PossibleNullReferenceException
            // See docs. Returns a collection or empty collection.
            var rows = plotTable.Rows;

            var isFirst = false;

            foreach (DataRow row in rows)
            {
                if (!isFirst)
                {
                    isFirst = true;
                    continue;
                }

                var excelRow = new ExcelEventRow
                {
                    Sid = row[0] as string,
                    Name = row[1] as string,
                    GoalDescription = row[2] as string,
                    Location = row[3] as string,
                    BeforeCombatAftermath = row[4] as string,
                    AfterCombatAftermath = row[5] as string,
                    ParentSids = row[6] as string
                };

                excelRows.Add(excelRow);
            }

            return excelRows;
        }

        private static IEnumerable<ExcelTextFragmentRow> ReadTextFragmentsFromExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelTextFragmentRow>();
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();

            var plotTable = result.Tables["Texts"];

            // ReSharper disable once PossibleNullReferenceException
            // See docs. Returns a collection or empty collection.
            var rows = plotTable.Rows;

            var isFirst = false;

            foreach (DataRow row in rows)
            {
                if (!isFirst)
                {
                    isFirst = true;
                    continue;
                }

                var excelRow = new ExcelTextFragmentRow
                {
                    EventSid = row[0] as string,
                    Speaker = row[1] as string,
                    Text = row[2] as string,
                    Index = (int)((double?)row[3]).GetValueOrDefault(),
                    CombatPosition = (int)((double?)row[4]).GetValueOrDefault()
                };

                excelRows.Add(excelRow);
            }

            return excelRows;
        }
    }
}