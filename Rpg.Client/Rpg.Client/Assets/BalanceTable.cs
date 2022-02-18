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

        public BalanceTable()
        {
            _dict = new Dictionary<UnitName, BalanceTableRecord>();

            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "Rpg.Client.Resources.Balance.json";

            using var stream = assembly.GetManifestResourceStream(RESOURCE_NAME);
            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                var result = reader.ReadToEnd();

                var list = JsonSerializer.Deserialize<BalanceTableRecord[]>(result, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                });

                foreach (var item in list)
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