using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core.ProgressStorage;

namespace Rpg.Client.Core
{
    internal sealed class GlobeProvider
    {
        private const string SAVE_FILE_TEMPLATE = "save-{0}.json";
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
            if (!Directory.Exists(_storagePath))
            {
                return false;
            }

            return !IsDirectoryEmpty(_storagePath);
        }

        public void GenerateNew()
        {
            var storyPointCatalog = new StoryPointCatalog();

            var globe = new Globe(_biomeGenerator, new Player());

            InitStartStoryPoint(globe, storyPointCatalog);

            var startUnits = CreateStartUnits();
            for (var slotIndex = 0; slotIndex < startUnits.Length; slotIndex++)
            {
                globe.Player.Party.Slots[slotIndex].Unit = startUnits[slotIndex];
            }

            CreateStartCombat(globe);

            Globe = globe;
        }

        public IReadOnlyCollection<SaveShortInfo> GetSaves()
        {
            if (!Directory.Exists(_storagePath))
            {
                return Array.Empty<SaveShortInfo>();
            }

            var files = Directory.EnumerateFiles(_storagePath);

            var saves = new List<SaveShortInfo>();
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var jsonSave = JsonSerializer.Deserialize<SaveShortInfo>(content);
                Debug.Assert(jsonSave is not null);
                jsonSave.FileName = Path.GetFileName(file);

                saves.Add(jsonSave);
            }

            return saves;
        }

        public SaveDto GetStoredData(string saveName)
        {
            var storageFile = Path.Combine(_storagePath, saveName);

            var json = File.ReadAllText(storageFile);

            var saveDataDto = JsonSerializer.Deserialize<SaveDto>(json);

            if (saveDataDto is null)
            {
                throw new InvalidOperationException("Error during loading the last save.");
            }

            return saveDataDto;
        }

        public void LoadGlobe(string saveName)
        {
            var saveDataDto = GetStoredData(saveName);

            var progressDto = saveDataDto.Progress;

            var player = new Player(saveDataDto.Name);

            Globe = new Globe(_biomeGenerator, player);

            if (progressDto.Player is not null)
            {
                LoadPlayerCharacters(progressDto.Player);
                LoadPlayerAbilities(progressDto.Player);

                LoadPlayerResources(progressDto.Player.Resources, Globe.Player.Inventory);
                LoadPlayerKnownMonsters(progressDto.Player, _unitSchemeCatalog, Globe.Player);
            }

            LoadEvents(progressDto.Events);

            LoadBiomes(progressDto.Biomes, Globe.Biomes);

            Globe.UpdateNodes(_dice, _eventCatalog);
        }

        public void StoreCurrentGlobe()
        {
            var player = new PlayerDto
            {
                Group = GetPlayerGroupToSave(Globe.Player.Party.GetUnits()),
                Pool = GetPlayerGroupToSave(Globe.Player.Pool.Units),
                Resources = GetPlayerResourcesToSave(Globe.Player.Inventory),
                KnownMonsterSids = GetKnownMonsterSids(Globe.Player.KnownMonsters),
                Abilities = Globe.Player.Abilities.Select(x => x.ToString()).ToArray(),
            };


            var progress = new ProgressDto
            {
                Player = player,
                Events = GetUsedEventDtos(_eventCatalog.Events),
                Biomes = GetBiomeDtos(Globe.Biomes)
            };

            var saveName = GetSaveName(Globe.Player.Name);

            var saveDataString = CreateSaveData(Globe.Player.Name, progress);

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            var storageFile = Path.Combine(_storagePath, saveName);
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

        private void CreateStartCombat(Globe globe)
        {
            var startBiome = globe.Biomes.Single(x => x.Type == BiomeType.Slavic);
            _biomeGenerator.CreateStartCombat(startBiome);
        }

        private Unit[] CreateStartUnits()
        {
            return new[]
            {
                new Unit(_unitSchemeCatalog.Heroes[UnitName.Swordsman], level: 1)
                {
                    IsPlayerControlled = true
                },
                new Unit(_unitSchemeCatalog.Heroes[UnitName.Comissar], level: 1)
                {
                    IsPlayerControlled = true
                },
                new Unit(_unitSchemeCatalog.Heroes[UnitName.Assaulter], level: 1)
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
                    Type = biome.Type,
                    IsComplete = biome.IsComplete,
                    Nodes = GetNodeDtos(biome)
                };
            }
        }

        private static EquipmentDto[] GetCharacterEquipmentToSave(Unit unit)
        {
            var equipmentDtoList = new List<EquipmentDto>();

            foreach (var equipment in unit.Equipments)
            {
                var dto = new EquipmentDto
                {
                    Sid = equipment.Scheme.Sid.ToString(),
                    Level = equipment.Level
                };

                equipmentDtoList.Add(dto);
            }

            return equipmentDtoList.ToArray();
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
                    Hp = unit.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.Current,
                    Level = unit.Level,
                    //ManaPool = unit.ManaPool,
                    Equipments = GetCharacterEquipmentToSave(unit)
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

        private string GetSaveName(string playerName)
        {
            var saves = GetSaves();
            var currentSave = saves.SingleOrDefault(x => x.PlayerName == playerName);
            if (currentSave is null)
            {
                var randomFileName = Path.GetRandomFileName();
                return string.Format(SAVE_FILE_TEMPLATE, randomFileName);
            }

            return currentSave.FileName;
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

        private static void InitStartStoryPoint(Globe globe, StoryPointCatalog storyPointCatalog)
        {
            var startStoryPoints = storyPointCatalog.Init(globe);
            foreach (var storyPoint in startStoryPoints)
            {
                globe.AddActiveStoryPoint(storyPoint);
            }
        }

        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private static void LoadBiomes(IEnumerable<BiomeDto?>? biomeDtoList, IEnumerable<Biome> biomes)
        {
            if (biomeDtoList is null)
            {
                return;
            }

            var biomeMaterializedList = biomes as Biome[] ?? biomes.ToArray();
            foreach (var biomeDto in biomeDtoList)
            {
                if (biomeDto is null)
                {
                    continue;
                }
                var targetBiome = biomeMaterializedList.Single(x => x.Type == biomeDto.Type);
                targetBiome.IsComplete = biomeDto.IsComplete;

                LoadNodes(targetBiome, biomeDto);
            }
        }

        private static void LoadCharacterEquipments(Unit unit, EquipmentDto[]? unitDtoEquipments)
        {
            if (unitDtoEquipments is null)
            {
                // Old version of the saves.
                return;
            }

            foreach (var dto in unitDtoEquipments)
            {
                var equipment = unit.Equipments.SingleOrDefault(x => x.Scheme.Sid.ToString() == dto.Sid);

                if (equipment is null)
                {
                    Debug.Fail($"{dto.Sid} is invalid equipment in the storage. Make migration of the save.");
                    continue;
                }

                for (var i = 0; i < equipment.Level; i++)
                {
                    equipment.LevelUp();
                }
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
                var unitScheme = _unitSchemeCatalog.Heroes[unitName];

                var unit = new Unit(unitScheme, unitDto.Level)
                {
                    IsPlayerControlled = true
                };

                LoadCharacterEquipments(unit, unitDto.Equipments);

                //if (unitDto.ManaPool is not null)
                //{
                //    unit.ManaPool = Math.Min(unitDto.ManaPool.Value, unit.RedEnergyPoolSize);
                //}

                units.Add(unit);
            }

            return units;
        }

        private static void LoadPlayerKnownMonsters(PlayerDto playerDto, IUnitSchemeCatalog unitSchemeCatalog,
            Player player)
        {
            player.KnownMonsters.Clear();

            if (playerDto.KnownMonsterSids is null)
            {
                return;
            }

            foreach (var monsterSid in playerDto.KnownMonsterSids)
            {
                var monsterScheme = unitSchemeCatalog.AllMonsters.SingleOrDefault(x => x.Name.ToString() == monsterSid);

                if (monsterScheme is null)
                {
                    Debug.Fail("Make migration of the save");
                }
                else
                {
                    player.KnownMonsters.Add(monsterScheme);
                }
            }
        }

        private static void LoadPlayerResources(ResourceDto?[]? resources, IReadOnlyCollection<ResourceItem> inventory)
        {
            if (resources is null)
            {
                return;
            }

            foreach (var resourceDto in resources)
            {
                if (resourceDto is null)
                {
                    continue;
                }

                var resource = inventory.SingleOrDefault(x => x.Type.ToString() == resourceDto.Type);
                if (resource is null)
                {
                    Debug.Fail("Every resouce in inventory must be same as in the save. Make migration of the save.");
                    continue;
                }

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

        internal class SaveDto
        {
            public string Name { get; init; }
            public ProgressDto Progress { get; init; }
            public DateTime UpdateTime { get; init; }
        }
    }
}