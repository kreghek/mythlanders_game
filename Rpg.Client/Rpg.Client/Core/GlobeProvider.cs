using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Core;
using Rpg.Client.Core.ProgressStorage;

namespace Rpg.Client.Models
{
    internal sealed class GlobeProvider
    {
        private const string SAVE_JSON = "save.json";

        private readonly IDice _dice;

        private readonly string _saveFilePath;

        private Globe? _globe;

        public GlobeProvider(IDice dice)
        {
            _dice = dice;

            var binPath = AppContext.BaseDirectory;
            _saveFilePath = Path.Combine(binPath, SAVE_JSON);
        }

        public Globe Globe
        {
            get
            {
                if (_globe is null)
                {
                    throw new InvalidOperationException("Globe is not initialized.");
                }

                return _globe;
            }
            private set => _globe = value;
        }

        public bool CheckExistsSave()
        {
            if (File.Exists(_saveFilePath))
            {
                return true;
            }

            Debug.WriteLine($"Not found a save file by provided path: {_saveFilePath}");

            return false;
        }

        public void GenerateNew()
        {
            var globe = new Globe
            {
                Player = new Player
                {
                    Group = new Group
                    {
                        Units = CreateStartUnits()
                    }
                }
            };

            Globe = globe;
        }

        public bool LoadGlobe()
        {
            if (!CheckExistsSave())
            {
                return false;
            }

            var json = File.ReadAllText(_saveFilePath);

            var lastSave = JsonSerializer.Deserialize<ProgressDto>(json);

            if (lastSave is null)
            {
                throw new InvalidOperationException("Error during loading the last save.");
            }

            Globe = new Globe
            {
                Player = new Player
                {
                    Pool = LoadPlayerGroup(lastSave.Player.Pool),
                    Group = LoadPlayerGroup(lastSave.Player.Group)
                }
            };

            Globe.UpdateNodes(_dice);

            return true;
        }

        public void StoreGlobe()
        {
            var progress = new ProgressDto
            {
                Player = new PlayerDto
                {
                    Group = GetGroupToSave(Globe.Player.Group.Units),
                    Pool = GetGroupToSave(Globe.Player.Pool.Units)
                }
            };
            var serializedSave = JsonSerializer.Serialize(progress);
            File.WriteAllText(_saveFilePath, serializedSave);
        }

        private static Unit[] CreateStartUnits()
        {
            return new[]
            {
                new Unit(UnitSchemeCatalog.SwordmanHero, 1)
                {
                    IsPlayerControlled = true,
                    EquipmentLevel = 1
                }
            };
        }

        private static GroupDto GetGroupToSave(IEnumerable<Unit> units)
        {
            var unitDtos = units.Select(
                unit => new UnitDto
                {
                    SchemeSid = unit.UnitScheme.Name,
                    Hp = unit.Hp,
                    Xp = unit.Xp,
                    Level = unit.Level
                });

            var groupDto = new GroupDto
            {
                Units = unitDtos
            };

            return groupDto;
        }

        private static Group LoadPlayerGroup(GroupDto groupDto)
        {
            var units = new List<Unit>();
            foreach (var unitDto in groupDto.Units)
            {
                var unitScheme = UnitSchemeCatalog.PlayerUnits[unitDto.SchemeSid];
                var unit = new Unit(unitScheme, unitDto.Level, unitDto.EquipmentLevel, unitDto.Xp, unitDto.EquipmentItems)
                {
                    IsPlayerControlled = true
                };
                units.Add(unit);
            }

            var restoredGroup = new Group
            {
                Units = units
            };

            return restoredGroup;
        }
    }
}