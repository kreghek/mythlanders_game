using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using ExcelDataReader;

namespace PlotConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            var filePath = "Ewar - Plot.xlsx";
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
                    var excelRows = new List<ExcelRow>();
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
                            CombatPosition = (int)((double?)row[4]).GetValueOrDefault()
                        };

                        excelRows.Add(excelRow);
                    }

                    
                }
            }
        }
    }
}
