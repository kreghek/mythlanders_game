﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Core.Balance;

namespace Client.Assets;

internal class BalanceTable : IBalanceTable
{
    private readonly Dictionary<string, BalanceTableRecord> _dict;

    private readonly CommonUnitBasics _unitBasics;

    public BalanceTable()
    {
        _dict = new Dictionary<string, BalanceTableRecord>();

        var assembly = Assembly.GetExecutingAssembly();
        const string UNIT_BALANCE_RESOURCE_NAME = "Client.Resources.Balance.json";

        using var stream = assembly.GetManifestResourceStream(UNIT_BALANCE_RESOURCE_NAME);
        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            var balanceData = JsonSerializer.Deserialize<BalanceData>(result, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });

            if (balanceData is null)
            {
                throw new InvalidOperationException("Balance data can't be null.");
            }

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

    public BalanceTableRecord GetRecord(string unitName)
    {
        return _dict[unitName];
    }

    public CommonUnitBasics GetCommonUnitBasics()
    {
        return _unitBasics;
    }
}