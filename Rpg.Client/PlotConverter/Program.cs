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

        private static List<EventDto> ConventExcelRowsToObjectGraph(List<ExcelEventRow> excelEventRows,
            List<ExcelTextFragmentRow> excelRows)
        {
            var eventGrouped = excelRows.GroupBy(x => x.EventSid);

            var eventDtoList = new List<EventDto>();
            foreach (var excelEventGroup in eventGrouped)
            {
                var excelEvent = excelEventRows.Single(x => x.Sid == excelEventGroup.Key);

                var eventDto = new EventDto
                {
                    Name = excelEvent.Name,
                    Location = excelEvent.Location,
                    Aftermath = excelEvent.Aftermath
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

        private static void Main(string[] args)
        {
            var excelEventRows = ReadEventsFromExcel("Ewar - Plot.xlsx");
            var excelTextFragmentsRows = ReadTextFragmentsFromExcel("Ewar - Plot.xlsx");

            var eventDtoList = ConventExcelRowsToObjectGraph(excelEventRows, excelTextFragmentsRows);

            var serialized = JsonSerializer.Serialize(eventDtoList, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });

            File.WriteAllLines("plot-ru.json", new[] { serialized });
        }

        private static List<ExcelEventRow> ReadEventsFromExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelEventRow>();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    var plotTable = result.Tables["Events"];

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
                            Location = row[2] as string,
                            Aftermath = row[3] as string
                        };

                        excelRows.Add(excelRow);
                    }
                }
            }

            return excelRows;
        }

        private static List<ExcelTextFragmentRow> ReadTextFragmentsFromExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelTextFragmentRow>();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    var plotTable = result.Tables["Texts"];

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
                }
            }

            return excelRows;
        }
    }
}