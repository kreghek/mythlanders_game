using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Client.Core.Heroes;
using Client.Core.ProgressStorage;

using CombatDicesTeam.Dices;

using GameAssets.Combats;

namespace Client.Core;

internal sealed class GlobeProvider
{
    private const string SAVE_FILE_TEMPLATE = "save-{0}.json";

    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;

    private readonly string _storagePath;
    private readonly IStoryPointInitializer _storyPointInitializer;
    private readonly IUnitSchemeCatalog _unitSchemeCatalog;

    private Globe? _globe;

    public GlobeProvider(IDice dice,
        IUnitSchemeCatalog unitSchemeCatalog,
        IEventCatalog eventCatalog,
        IStoryPointInitializer storyPointInitializer)
    {
        _dice = dice;
        _unitSchemeCatalog = unitSchemeCatalog;
        _eventCatalog = eventCatalog;
        _storyPointInitializer = storyPointInitializer;
        var binPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storagePath = Path.Combine(binPath, "CDT", "Testament");
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

    public void GenerateFree(string[] heroes)
    {
        var globe = new Globe(new Player());

        AssignFreeHeroes(globe, heroes);

        Globe = globe;
    }

    public void GenerateNew()
    {
        var globe = new Globe(new Player());

        InitStartStoryPoint(globe, _storyPointInitializer);
        AssignStartHeroes(globe);

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

        return saveDataDto is null
            ? throw new InvalidOperationException("Error during loading the last save.")
            : saveDataDto;
    }

    public void LoadGlobe(string saveName)
    {
        var saveDataDto = GetStoredData(saveName);

        var progressDto = saveDataDto.Progress;

        var player = new Player(saveDataDto.Name);

        Globe = new Globe(player);

        if (progressDto.Player is not null)
        {
            LoadPlayerCharacters(progressDto.Player);
            LoadPlayerAbilities(progressDto.Player);

            LoadPlayerResources(progressDto.Player.Resources, Globe.Player.Inventory);
            LoadPlayerKnownMonsters(progressDto.Player, _unitSchemeCatalog, Globe.Player);
        }

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
            Abilities = Globe.Player.Abilities.Select(x => x.ToString()).ToArray()
        };

        var progress = new ProgressDto
        {
            Player = player
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

    private void AssignFreeHeroes(Globe globe, string[] heroes)
    {
        var startHeroes = CreateFreeHeroes(heroes.Select(x => Enum.Parse<UnitName>(x, true)).ToArray());
        for (var slotIndex = 0; slotIndex < startHeroes.Length; slotIndex++)
        {
            globe.Player.MoveToParty(startHeroes[slotIndex], slotIndex);
        }

        var startPoolHeroes = CreateStartPoolHeroes();
        foreach (var hero in startPoolHeroes)
        {
            globe.Player.Pool.AddNewUnit(hero);
        }
    }

    private void AssignStartHeroes(Globe globe)
    {
        var startHeroes = CreateStartHeroes();
        for (var slotIndex = 0; slotIndex < startHeroes.Length; slotIndex++)
        {
            globe.Player.MoveToParty(startHeroes[slotIndex], slotIndex);
        }

        var startPoolHeroes = CreateStartPoolHeroes();
        foreach (var hero in startPoolHeroes)
        {
            globe.Player.Pool.AddNewUnit(hero);
        }
    }

    private Hero[] CreateFreeHeroes(UnitName[] heroes)
    {
        var startHeroes = heroes.Select(CreateStartHero).ToArray();

        return startHeroes;
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

    private Hero CreateStartHero(UnitName heroName)
    {
        return new Hero(_unitSchemeCatalog.Heroes[heroName], level: 1)
        {
            IsPlayerControlled = true
        };
    }

    private Hero[] CreateStartHeroes()
    {
        var startHeroNames = new[]
        {
            UnitName.Swordsman,
            UnitName.Partisan,
            UnitName.Assaulter
        };

        var startHeroes = startHeroNames.Select(CreateStartHero).ToArray();

        return startHeroes;
    }

    private Hero[] CreateStartPoolHeroes()
    {
        var startHeroNames = Array.Empty<UnitName>();

        var startHeroes = startHeroNames.Select(CreateStartHero).ToArray();

        return startHeroes;
    }

    private static EquipmentDto[] GetCharacterEquipmentToSave(Hero unit)
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

    private static GroupDto GetPlayerGroupToSave(IEnumerable<Hero> units)
    {
        var unitDtos = units.Select(
            unit => new PlayerUnitDto
            {
                SchemeSid = unit.UnitScheme.Name.ToString(),
                Hp = unit.Stats.Single(x => x.Type == CombatantStatTypes.HitPoints).Value.Current,
                Level = unit.Level,
                Equipments = GetCharacterEquipmentToSave(unit)
            });

        var groupDto = new GroupDto
        {
            Units = unitDtos
        };

        return groupDto;
    }

    private static ResourceDto[] GetPlayerResourcesToSave(Inventory inventory)
    {
        // return inventory.Select(x => new ResourceDto
        // {
        //     Amount = x.Amount,
        //     Type = x.Type.ToString()
        // }).ToArray();

        throw new Exception();
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

    private static void InitStartStoryPoint(Globe globe, IStoryPointInitializer storyPointCatalog)
    {
        storyPointCatalog.Init(globe);
    }

    private static bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    private static void LoadCharacterEquipments(Hero unit, EquipmentDto[]? unitDtoEquipments)
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
            slot.Hero = null;
        }

        for (var slotIndex = 0; slotIndex < loadedParty.Count; slotIndex++)
        {
            var unit = loadedParty[slotIndex];
            Globe.Player.Party.Slots[slotIndex].Hero = unit;
        }

        var loadedPool = LoadPlayerGroup(lastSavePlayer.Pool);

        foreach (var unit in loadedPool)
        {
            Globe.Player.Pool.AddNewUnit(unit);
        }
    }

    private List<Hero> LoadPlayerGroup(GroupDto groupDto)
    {
        var units = new List<Hero>();
        foreach (var unitDto in groupDto.Units)
        {
            var unitName = (UnitName)Enum.Parse(typeof(UnitName), unitDto.SchemeSid);
            var unitScheme = _unitSchemeCatalog.Heroes[unitName];

            var unit = new Hero(unitScheme, unitDto.Level)
            {
                IsPlayerControlled = true
            };

            LoadCharacterEquipments(unit, unitDto.Equipments);

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

    private static void LoadPlayerResources(ResourceDto?[]? resources, Inventory inventory)
    {
        if (resources is null)
        {
            return;
        }

        // foreach (var resourceDto in resources)
        // {
        //     if (resourceDto is null)
        //     {
        //         continue;
        //     }
        //
        //     var resource = inventory.SingleOrDefault(x => x.Type.ToString() == resourceDto.Type);
        //     if (resource is null)
        //     {
        //         Debug.Fail("Every resouce in inventory must be same as in the save. Make migration of the save.");
        //         continue;
        //     }
        //
        //     resource.Amount = resourceDto.Amount;
        // }

        throw new Exception();
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