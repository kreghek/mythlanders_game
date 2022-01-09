﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Core.ProgressStorage;

namespace Rpg.Client.Core
{
    internal sealed class GlobeProvider
    {
        private const string SAVE_JSON = "save.json";
        private readonly IBiomeGenerator _biomeGenerator;

        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;

        private readonly string _saveFilePath;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private Globe? _globe;

        public GlobeProvider(IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IBiomeGenerator biomeGenerator,
            IEventCatalog eventCatalog)
        {
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
            _biomeGenerator = biomeGenerator;
            _eventCatalog = eventCatalog;
            var binPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _saveFilePath = Path.Combine(binPath, SAVE_JSON);
        }

        public (int Width, int Height)? ChoisedUserMonitorResolution { get; set; } = null;

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
            var globe = new Globe(_biomeGenerator)
            {
                Player = new Player()
            };

            var startUnits = CreateStartUnits();
            for (var slotIndex = 0; slotIndex < startUnits.Length; slotIndex++)
            {
                globe.Player.Party.Slots[slotIndex].Unit = startUnits[slotIndex];
            }

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

            Globe = new Globe(_biomeGenerator)
            {
                Player = new Player()
            };

            if (lastSave.Player is not null)
            {
                LoadPlayerCharacters(lastSave.Player);
            }

            LoadPlayerResources(Globe.Player.Inventory, lastSave.Player.Resources);
            LoadPlayerKnownMonsters(lastSave.Player, _unitSchemeCatalog, Globe.Player);

            LoadEvents(lastSave.Events);

            LoadBiomes(lastSave.Biomes, Globe.Biomes);

            Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);

            return true;
        }

        private void LoadPlayerKnownMonsters(PlayerDto playerDto, IUnitSchemeCatalog unitSchemeCatalog, Player player)
        {
            player.KnownMonsters.Clear();

            if (playerDto.KnownMonsterSids is null)
            {
                return;
            }

            foreach (var monsterSid in playerDto.KnownMonsterSids)
            {
                var monsterScheme = unitSchemeCatalog.AllMonsters.Single(x => x.Name.ToString() == monsterSid);
                player.KnownMonsters.Add(monsterScheme);
            }
        }

        private void LoadPlayerResources(IReadOnlyCollection<ResourceItem> inventory, ResourceDto[] resources)
        {
            if (resources is null)
            {
                return;
            }

            foreach (var resourceDto in resources)
            {
                var resource = inventory.Single(x => x.Type == resourceDto.Type);
                resource.Amount = resourceDto.Amount;
            }
        }

        public void StoreGlobe()
        {
            PlayerDto? player = null;
            if (Globe.Player != null)
            {
                player = new PlayerDto
                {
                    Group = GetPlayerGroupToSave(Globe.Player.Party.GetUnits()),
                    Pool = GetPlayerGroupToSave(Globe.Player.Pool.Units),
                    Resources = GetPlayerResourcesToSave(Globe.Player.Inventory),
                    KnownMonsterSids = GetKnownMonsterSids(Globe.Player.KnownMonsters)
                };
            }

            var progress = new ProgressDto
            {
                Player = player,
                Events = GetUsedEventDtos(_eventCatalog.Events),
                Biomes = GetBiomeDtos(Globe.Biomes)
            };
            var serializedSave =
                JsonSerializer.Serialize(progress, options: new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_saveFilePath, serializedSave);
        }

        private static string[] GetKnownMonsterSids(IList<UnitScheme> knownMonsters)
        {
            return knownMonsters.Select(x => x.Name.ToString()).ToArray();
        }

        private static ResourceDto[] GetPlayerResourcesToSave(IReadOnlyCollection<ResourceItem> inventory)
        {
            return inventory.Select(x => new ResourceDto
            {
                Amount = x.Amount,
                Type = x.Type
            }).ToArray();
        }

        private Unit[] CreateStartUnits()
        {
            return new[]
            {
                new Unit(_unitSchemeCatalog.PlayerUnits[UnitName.Berimir], level: 1, equipmentLevel: 1)
                {
                    IsPlayerControlled = true
                }
            };
        }

        private static IEnumerable<BiomeDto> GetBiomeDtos(IEnumerable<Biome> biomes)
        {
            foreach (var biome in biomes)
            {
                yield return new BiomeDto
                {
                    Level = biome.Level,
                    Type = biome.Type,
                    IsComplete = biome.IsComplete,
                    IsAvailable = biome.IsAvailable,
                    Nodes = GetNodeDtos(biome)
                };
            }
        }

        private static IEnumerable<GlobeNodeDto?> GetNodeDtos(Biome biome)
        {
            foreach (var node in biome.Nodes)
            {
                var nodeDto = new GlobeNodeDto
                {
                    Sid = node.Sid,
                    IsAvailable = node.IsAvailable
                };

                yield return nodeDto;
            }
        }

        private static GroupDto GetPlayerGroupToSave(IEnumerable<Unit> units)
        {
            var unitDtos = units.Select(
                unit => new PlayerUnitDto
                {
                    SchemeSid = unit.UnitScheme.Name.ToString(),
                    Hp = unit.HitPoints,
                    Level = unit.Level,
                    EquipmentLevel = unit.EquipmentLevel,
                    ManaPool = unit.ManaPool
                });

            var groupDto = new GroupDto
            {
                Units = unitDtos
            };

            return groupDto;
        }

        private static IEnumerable<EventDto> GetUsedEventDtos(IEnumerable<Event> events)
        {
            foreach (var eventItem in events)
            {
                if (eventItem.Counter <= 0)
                {
                    continue;
                }

                var dto = new EventDto
                {
                    Sid = eventItem.Sid,
                    Counter = eventItem.Counter
                };

                yield return dto;
            }
        }

        private static void LoadBiomes(IEnumerable<BiomeDto?>? biomeDtoList, IEnumerable<Biome> biomes)
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

                LoadNodes(targetBiome, biomeDto);
            }
        }

        private void LoadEvents(IEnumerable<EventDto?>? eventDtoList)
        {
            foreach (var eventItem in _eventCatalog.Events)
            {
                eventItem.Counter = 0;
            }

            if (eventDtoList is null)
            {
                return;
            }

            foreach (var eventDto in eventDtoList)
            {
                if (eventDto is null)
                {
                    continue;
                }

                var eventItem = _eventCatalog.Events.Single(x => x.Sid == eventDto.Sid);
                eventItem.Counter = eventDto.Counter;
            }
        }

        private static void LoadNodes(Biome targetBiome, BiomeDto biomeDto)
        {
            if (biomeDto.Nodes is null)
            {
                Debug.Fail("The globe nodes must be defined in saved file.");
                return;
            }

            foreach (var nodeDto in biomeDto.Nodes)
            {
                if (nodeDto is null)
                {
                    Debug.Fail("The node dto cannot be null.");
                    continue;
                }

                var targetNode = targetBiome.Nodes.SingleOrDefault(x => x.Sid == nodeDto.Sid);
                if (targetNode is not null)
                {
                    targetNode.IsAvailable = nodeDto.IsAvailable;
                }
            }
        }

        private void LoadPlayerCharacters(PlayerDto lastSavePlayer)
        {
            var loadedParty = LoadPlayerGroup(lastSavePlayer.Group);
            foreach (var slot in Globe.Player.Party.Slots)
            {
                slot.Unit = null;
            }

            for (var slotIndex = 0; slotIndex < loadedParty.Count; slotIndex++)
            {
                var unit = loadedParty[slotIndex];
                Globe.Player.Party.Slots[slotIndex].Unit = unit;
            }

            var loadedPool = LoadPlayerGroup(lastSavePlayer.Pool);
            Globe.Player.Pool.Units = loadedPool;
        }

        private List<Unit> LoadPlayerGroup(GroupDto groupDto)
        {
            var units = new List<Unit>();
            foreach (var unitDto in groupDto.Units)
            {
                var unitName = (UnitName)Enum.Parse(typeof(UnitName), unitDto.SchemeSid);
                var unitScheme = _unitSchemeCatalog.PlayerUnits[unitName];

                Debug.Assert(unitDto.EquipmentLevel > 0, "The player unit's equipment level always bigger that zero.");

                var unit = new Unit(unitScheme, unitDto.Level, unitDto.EquipmentLevel)
                {
                    IsPlayerControlled = true
                };

                if (unitDto.ManaPool is not null)
                {
                    unit.ManaPool = Math.Min(unitDto.ManaPool.Value, unit.ManaPoolSize);
                }

                units.Add(unit);
            }

            return units;
        }
    }
}