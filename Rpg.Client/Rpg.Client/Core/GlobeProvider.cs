using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Core;
using Rpg.Client.Models.Save;

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

            var lastSave = JsonSerializer.Deserialize<SaveDto>(json);

            if (lastSave is null)
            {
                throw new InvalidOperationException("Error during loading the last save.");
            }

            Globe = new Globe
            {
                Player = new Player
                {
                    Pool = GetSavedGroup(lastSave.Player.Pool),
                    Group = GetSavedGroup(lastSave.Player.Group)
                }
            };

            Globe.UpdateNodes(_dice);

            return true;
        }

        public void StoreGlobe()
        {
            var save = new SaveDto
            {
                Combats = new List<Core.Combat>(),
                Player = new PlayerParty
                {
                    Group = GetGroupToSave(Globe.Player.Group.Units),
                    Pool = GetGroupToSave(Globe.Player.Pool.Units)
                }
            };
            var serializedSave = JsonSerializer.Serialize(save);
            File.WriteAllText(_saveFilePath, serializedSave);
        }

        private static GroupUnits GetGroupToSave(IEnumerable<Unit> units)
        {
            var savedUnits = units.Select(
                unit => new UnitDto
                {
                    SchemeName = unit.UnitScheme.Name,
                    Hp = unit.Hp,
                    SkillSids = unit.Skills.Select(x => x.Sid),
                    Xp = unit.Xp,
                    Level = unit.Level
                });

            var savedGroup = new GroupUnits
            {
                Units = savedUnits
            };

            return savedGroup;
        }

        private static Group GetSavedGroup(GroupUnits groupUnits)
        {
            var restoredUnits = groupUnits.Units.Select(
                                              unit => UnitSchemeCatalog.PlayerUnits.TryGetValue(
                                                  unit.SchemeName,
                                                  out var unitScheme)
                                                  ? new Unit(unitScheme, unit.Level)
                                                  : null)
                                          .Where(x => x != null)
                                          .Cast<Unit>();
            var restoredGroup = new Group
            {
                Units = restoredUnits
            };

            return restoredGroup;
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
    }
}