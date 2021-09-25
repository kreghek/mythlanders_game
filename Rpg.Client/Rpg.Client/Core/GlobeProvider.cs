﻿using System;
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

            Globe = new Globe();
            if (lastSave.Player != null)
            {
                Globe.Player = new Player
                {
                    Pool = LoadPlayerGroup(lastSave.Player.Pool),
                    Group = LoadPlayerGroup(lastSave.Player.Group)
                };
            }

            LoadEvents(lastSave.Events);

            LoadBiomes(lastSave.Biomes, Globe.Biomes);

            Globe.UpdateNodes(_dice);

            return true;
        }

        public void StoreGlobe()
        {
            PlayerDto? player = null;
            if (Globe.Player != null)
            {
                player = new PlayerDto
                {
                    Group = GetPlayerGroupToSave(Globe.Player.Group.Units),
                    Pool = GetPlayerGroupToSave(Globe.Player.Pool.Units)
                };
            }
            var progress = new ProgressDto
            {
                Player = player,
                Events = GetUsedEventDtos(EventCatalog.Events),
                Biomes = GetBiomeDtos(Globe.Biomes)
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

        private static IEnumerable<BiomeDto> GetBiomeDtos(IEnumerable<Core.Biome> biomes)
        {
            foreach (var biome in biomes)
            {
                yield return new BiomeDto
                {
                    Level = biome.Level,
                    Type = biome.Type,
                    IsComplete = biome.IsComplete,
                    IsAvailable = biome.IsAvailable
                };
            }
        }

        private static GroupDto GetPlayerGroupToSave(IEnumerable<Unit> units)
        {
            var unitDtos = units.Select(
                unit => new PlayerUnitDto
                {
                    SchemeSid = unit.UnitScheme.Name,
                    Hp = unit.Hp,
                    Xp = unit.Xp,
                    Level = unit.Level,
                    EquipmentItems = unit.EquipmentItems,
                    EquipmentLevel = unit.EquipmentLevel,
                    ManaPool = unit.ManaPool
                });

            var groupDto = new GroupDto
            {
                Units = unitDtos
            };

            return groupDto;
        }

        private static IEnumerable<EventDto> GetUsedEventDtos(IEnumerable<Core.Event> events)
        {
            foreach (var eventItem in events)
            {
                if (eventItem.Counter <= 0)
                {
                    continue;
                }

                var dto = new EventDto
                {
                    Sid = eventItem.Name,
                    Counter = eventItem.Counter
                };

                yield return dto;
            }
        }

        private static void LoadBiomes(IEnumerable<BiomeDto?>? biomeDtoList, IEnumerable<Core.Biome> biomes)
        {
            if (biomeDtoList is null)
            {
                return;
            }

            foreach (var biomeDto in biomeDtoList)
            {
                if (biomeDto is null)
                {
                    continue;
                }
                var targetBiome = biomes.Single(x => x.Type == biomeDto.Type);
                targetBiome.IsComplete = biomeDto.IsComplete;
                targetBiome.IsAvailable = biomeDto.IsAvailable;
                targetBiome.Level = biomeDto.Level;
            }
        }

        private static void LoadEvents(IEnumerable<EventDto?>? eventDtoList)
        {
            foreach (var eventItem in EventCatalog.Events)
            {
                eventItem.Counter = 0;
            }

            if (eventDtoList == null)
            {
                return;
            }

            foreach (var eventDto in eventDtoList)
            {
                if (eventDto is null)
                {
                    continue;
                }

                var eventItem = EventCatalog.Events.Single(x => x.Name == eventDto.Sid);
                eventItem.Counter = eventDto.Counter;
            }
        }

        private static Group LoadPlayerGroup(GroupDto groupDto)
        {
            var units = new List<Unit>();
            foreach (var unitDto in groupDto.Units)
            {
                var unitScheme = UnitSchemeCatalog.PlayerUnits[unitDto.SchemeSid];

                Debug.Assert(unitDto.EquipmentLevel > 0, "The player unit's equipment level always bigger that zero.");

                var unit = new Unit(unitScheme, unitDto.Level, unitDto.EquipmentLevel, unitDto.Xp,
                    unitDto.EquipmentItems)
                {
                    IsPlayerControlled = true
                };

                if (unitDto.ManaPool is not null)
                {
                    unit.ManaPool = unitDto.ManaPool.Value;
                }

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