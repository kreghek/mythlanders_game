using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using ExcelDataReader;

namespace BalanceConverter
{
    internal static class ExcelExtractor
    {
        public const string SOURCE_EVENTS_EXCEL = "Balance.xlsx";

        public static IReadOnlyCollection<UnitBasicRow> ReadUnitsBasicsFromExcel(string filePath, string sheetName)
        {
            return ReadRowsFromExcelInner(filePath, sheetName, firstRowIsHeader: false, row =>
            {
                return new UnitBasicRow
                {
                    Key = row[0] as string,
                    Value = GetFloatValue(row[1])
                };
            });
        }

        public static IReadOnlyCollection<UnitExcelRow> ReadUnitsRolesFromExcel(string filePath, string sheetName)
        {
            return ReadRowsFromExcelInner(filePath, sheetName, firstRowIsHeader: true, row =>
            {
                return new UnitExcelRow
                {
                    Sid = row[0] as string,
                    TankRank = GetFloatValue(row[1]),
                    DamageDealerRank = GetFloatValue(row[2]),
                    SupportRank = GetFloatValue(row[3]),
                    Type = row[4] as string,
                    Demo = row[5] as string
                };
            });
        }

        private static float GetFloatValue(object rowValue)
        {
            return (float)((double?)rowValue).GetValueOrDefault();
        }

        private static IReadOnlyCollection<TRow> ReadRowsFromExcelInner<TRow>(
            string filePath,
            string sheetName,
            bool firstRowIsHeader,
            Func<DataRow, TRow> mapper)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var excelRows = new List<TRow>();
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();

            var plotTable = result.Tables[sheetName];

            // ReSharper disable once PossibleNullReferenceException
            // See docs. Returns a collection or empty collection.
            var rows = plotTable.Rows;

            var isFirst = false;

            foreach (DataRow row in rows)
            {
                if (!isFirst && firstRowIsHeader)
                {
                    isFirst = true;
                    continue;
                }

                var excelRow = mapper(row);

                excelRows.Add(excelRow);
            }

            return excelRows;
        }
    }
}