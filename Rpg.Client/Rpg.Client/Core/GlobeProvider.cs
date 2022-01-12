using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Rpg.Client.Core.ProgressStorage;

namespace Rpg.Client.Core
{
    internal sealed class GlobeProvider
    {
        private const string SAVE_FILE_TEMPLATE = "save{0}.json";
        private readonly IBiomeGenerator _biomeGenerator;

        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;

        private readonly string _storagePath;
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
            _storagePath = Path.Combine(binPath, "CDT", "UpcomingPastJRPG");
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

        public bool CheckSavesExist()
        {
            if (!IsDirectoryEmpty(_storagePath))
            {
                return true;
            }

            Debug.WriteLine($"Not found a save file by provided path: {_storagePath}");

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

        public IReadOnlyCollection<SaveShortInfo> GetSaves()
        {
            var files = Directory.EnumerateFiles(_storagePath);

            var saves = new List<SaveShortInfo>();
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var jsonSave = JsonSerializer.Deserialize<SaveShortInfo>(content);
                jsonSave.FileName = Path.GetFileName(file);

                saves.Add(jsonSave);
            }

            return saves;
        }

        public void LoadGlobe(string saveName)
        {
            var storageFile = Path.Combine(_storagePath, saveName);

            var json = File.ReadAllText(storageFile);

            var saveDataDto = JsonSerializer.Deserialize<SaveDto>(json);

            if (saveDataDto is null)
            {
                throw new InvalidOperationException("Error during loading the last save.");
            }

            var progressDto = saveDataDto.Progress;

            Globe = new Globe(_biomeGenerator)
            {
                Player = new Player(saveDataDto.Name)
            };

            if (progressDto.Player is not null)
            {
                LoadPlayerCharacters(progressDto.Player);
                LoadPlayerAbilities(progressDto.Player);

                LoadPlayerResources(progressDto.Player.Resources, Globe.Player.Inventory);
                LoadPlayerKnownMonsters(progressDto.Player, _unitSchemeCatalog, Globe.Player);
            }

            LoadEvents(progressDto.Events);

            LoadBiomes(progressDto.Biomes, Globe.Biomes);

            Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
        }

        public void StoreCurrentGlobe()
        {
            PlayerDto? player = null;
            if (Globe.Player != null)
            {
                player = new PlayerDto
                {
                    Group = GetPlayerGroupToSave(Globe.Player.Party.GetUnits()),
                    Pool = GetPlayerGroupToSave(Globe.Player.Pool.Units),
                    Resources = GetPlayerResourcesToSave(Globe.Player.Inventory),
                    KnownMonsterSids = GetKnownMonsterSids(Globe.Player.KnownMonsters),
                    Abilities = Globe.Player.Abilities.Select(x => x.ToString()).ToArray()
                };
            }

            var progress = new ProgressDto
            {
                Player = player,
                Events = GetUsedEventDtos(_eventCatalog.Events),
                Biomes = GetBiomeDtos(Globe.Biomes)
            };

            var saveName = Path.GetRandomFileName();

            var saveDataString = CreateSaveData(Globe.Player.Name, progress);

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            var storageFile = Path.Combine(_storagePath, string.Format(SAVE_FILE_TEMPLATE, saveName));
            File.WriteAllText(storageFile, saveDataString);
        }

        private static string CreateSaveData(string saveName, ProgressDto progress)
        {
            var saveDto = new SaveDto
            {
                Name = saveName,
                UpdateTime = DateTime.UtcNow,
                Progress = progress
            };

            var serializedSaveData =
                JsonSerializer.Serialize(saveDto, options: new JsonSerializerOptions { WriteIndented = true });

            return serializedSaveData;
        }

        private Unit[] CreateStartUnits()
        {
            return new[]
            {
                new Unit(_unitSchemeCatalog.PlayerUnits[UnitName.Berimir], level: 1)
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

        private static string[] GetKnownMonsterSids(IList<UnitScheme> knownMonsters)
        {
            return knownMonsters.Select(x => x.Name.ToString()).ToArray();
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
                    ManaPool = unit.ManaPool
                });

            var groupDto = new GroupDto
            {
                Units = unitDtos
            };

            return groupDto;
        }

        private static ResourceDto[] GetPlayerResourcesToSave(IReadOnlyCollection<ResourceItem> inventory)
        {
            return inventory.Select(x => new ResourceDto
            {
                Amount = x.Amount,
                Type = x.Type.ToString()
            }).ToArray();
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

        private static bool IsDirectoryEmpty(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            return !Directory.EnumerateFileSystemEntries(path).Any();
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

        private void LoadPlayerAbilities(PlayerDto playerDto)
        {
            if (playerDto.Abilities is null)
            {
                return;
            }

            foreach (var playerAbilityDto in playerDto.Abilities)
            {
                if (Enum.TryParse<PlayerAbility>(playerAbilityDto, out var playerAbilityEnum))
                {
                    Globe.Player.AddPlayerAbility(playerAbilityEnum);
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

                var unit = new Unit(unitScheme, unitDto.Level)
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

        private void LoadPlayerResources(ResourceDto[] resources, IReadOnlyCollection<ResourceItem> inventory)
        {
            if (resources is null)
            {
                return;
            }

            foreach (var resourceDto in resources)
            {
                var resource = inventory.Single(x => x.Type.ToString() == resourceDto.Type);
                resource.Amount = resourceDto.Amount;
            }
        }

        public sealed class SaveShortInfo
        {
            public string FileName { get; set; }

            [JsonPropertyName(nameof(SaveDto.Name))]
            public string PlayerName { get; init; }

            public DateTime UpdateTime { get; init; }
        }

        private class SaveDto
        {
            public string Name { get; init; }
            public ProgressDto Progress { get; init; }
            public DateTime UpdateTime { get; init; }
        }
    }
}