using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;

using ExcelDataReader;

namespace PlotConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var excelRows = ReadFromExcel("Ewar - Plot.xlsx");

            var eventDtoList = ConventExcelRowsToObjectGraph(excelRows);

            var serialized = JsonSerializer.Serialize(eventDtoList);

            File.WriteAllLines("plot-ru.json", new[] { serialized });
        }

        private static List<EventDto> ConventExcelRowsToObjectGraph(List<ExcelRow> excelRows)
        {
            var eventGrouped = excelRows.GroupBy(x => x.Event);

            var eventDtoList = new List<EventDto>();
            foreach (var excelEventGroup in eventGrouped)
            {
                var eventDto = new EventDto
                {
                    Name = excelEventGroup.Key
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

        private static EventNodeDto BuildEventNodes(List<ExcelRow> excelEventNodes)
        {
            var openList = new List<ExcelRow>(excelEventNodes);

            EventNodeDto startNode = null;
            EventNodeDto currentNode = null;
            while (openList.Any())
            {
                var excelRowMinIndex = openList.Min(x => x.Index);
                var targetExcelRow = openList.Single(x=>x.Index == excelRowMinIndex);

                openList.Remove(targetExcelRow);

                var node = new EventNodeDto
                {
                    Speaker = targetExcelRow.Speaker,
                    Text = targetExcelRow.Text
                };

                if (startNode is null)
                {
                    startNode = node;
                }

                if (currentNode is null)
                {
                    currentNode = node;
                }
                else
                {
                    currentNode.NextNode = node;
                    currentNode = node;
                }
            }

            return startNode;
        }

        private static List<ExcelRow> ReadFromExcel(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelRow>();
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

                        var excelRow = new ExcelRow
                        {
                            Event = row[0] as string,
                            Speaker = row[1] as string,
                            Text = row[2] as string,
                            Index = (int)((double?)row[3]).GetValueOrDefault(),
                            CombatPosition = (string)row[4] == "до" ? -1 : 1
                        };

                        excelRows.Add(excelRow);
                    }
                }
            }

            return excelRows;
        }
    }
}
