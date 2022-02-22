using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal class BalanceTable : IBalanceTable
    {
        private readonly Dictionary<UnitName, BalanceTableRecord> _dict;

        private readonly CommonUnitBasics _unitBasics;

        public BalanceTable()
        {
            _dict = new Dictionary<UnitName, BalanceTableRecord>();

            var assembly = Assembly.GetExecutingAssembly();
            const string UNIT_BALANCE_RESOURCE_NAME = "Rpg.Client.Resources.Balance.json";

            using var stream = assembly.GetManifestResourceStream(UNIT_BALANCE_RESOURCE_NAME);
            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                var result = reader.ReadToEnd();

                var balanceData = JsonSerializer.Deserialize<BalanceData>(result, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                });

                _unitBasics = balanceData.UnitBasics;

                var unitRows = balanceData.UnitRows;

                foreach (var item in unitRows)
                {
                    _dict.Add(item.Sid, item);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public BalanceTableRecord GetRecord(UnitName unitName)
        {
            return _dict[unitName];
        }

        public CommonUnitBasics GetCommonUnitBasics()
        {
            return new CommonUnitBasics();
        }
    }
}