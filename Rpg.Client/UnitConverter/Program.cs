using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using ExcelDataReader;

namespace UnitConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var excelUnits = ReadUnitsFromExcel("Ewar - Units.xlsx");
            Console.WriteLine("Hello World!");
        }
        
        private static List<ExcelUnitRow> ReadUnitsFromExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<ExcelUnitRow>();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    var unitTable = result.Tables["Units"];

                    var rows = unitTable.Rows;

                    var isFirst = false;

                    foreach (DataRow row in rows)
                    {
                        if (!isFirst)
                        {
                            isFirst = true;
                            continue;
                        }

                        var excelRow = new ExcelUnitRow
                        {
                            NameSid = row[0] as string,
                            TankRole = (float)row[1],
                            DamageRole = (float)row[2],
                            SupportRole = (float)row[3],
                            IsMonster = (string)row[4] == "M",
                            AllowedLocations = (string)row[5]
                        };

                        excelRows.Add(excelRow);
                    }
                }
            }

            return excelRows;
        }
    }

    internal sealed class ExcelUnitRow
    {
        public string NameSid { get; init; }
        
        public float TankRole { get; init; }
        public float DamageRole { get; init; }
        public float SupportRole { get; init; }

        public string AllowedLocations { get; init; }

        public bool IsMonster { get; init; }
    }

    internal sealed class UnitScheme
    {
        public string NameSid { get; init; }
        
        public float TankRole { get; init; }
        public float DamageRole { get; init; }
        public float SupportRole { get; init; }

        public bool IsMonster { get; init; }
        public IEnumerable<SkillSetInfo> SkillSets { get; init; }
    }

    internal sealed class SkillSetInfo
    {
        public string[] SkillNameSids { get; init; }
        public int Level { get; init; }
    }
}