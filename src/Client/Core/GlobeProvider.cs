﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Client.Assets;
using Client.Assets.MonsterPerks;
using Client.Core.Heroes;
using Client.Core.ProgressStorage;

using Core.Props;

namespace Client.Core;

internal sealed class GlobeProvider
{
    private const string SAVE_FILE_TEMPLATE = "save-{0}.json";
    private readonly GameSettings _gameSettings;
    private readonly IMonsterPerkCatalog _monsterPerkCatalog;

    private readonly string _storagePath;
    private readonly IStoryPointInitializer _storyPointInitializer;
    private readonly ICharacterCatalog _unitSchemeCatalog;

    private Globe? _globe;

    public GlobeProvider(ICharacterCatalog unitSchemeCatalog,
        IStoryPointInitializer storyPointInitializer,
        IMonsterPerkCatalog monsterPerkCatalog,
        GameSettings gameSettings)
    {
        _unitSchemeCatalog = unitSchemeCatalog;
        _storyPointInitializer = storyPointInitializer;
        _monsterPerkCatalog = monsterPerkCatalog;
        _gameSettings = gameSettings;
        var binPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storagePath = Path.Combine(binPath, "CDT", "Mythlanders");
    }

    public GlobeProvider(ICharacterCatalog characterCatalog,
        IStoryPointInitializer storyPointInitializer,
        IMonsterPerkCatalog monsterPerkCatalog) : this(characterCatalog, storyPointInitializer, monsterPerkCatalog,
        new GameSettings())
    {
    }

    public (int Width, int Height)? ChosenUserMonitorResolution { get; set; }

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
        var globe = new Globe(new Player(), new CurrentGameFeatures());

        InitStartStoryPoint(globe, _storyPointInitializer);

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
            if (file.EndsWith("settings"))
            {
                continue;
            }

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

        Globe = new Globe(player, new CurrentGameFeatures());

        if (progressDto.Player is not null)
        {
            LoadHeroes(progressDto.Player);
            LoadPlayerAbilities(progressDto.Player);

            LoadPlayerResources(progressDto.Player.Resources, Globe.Player.Inventory);
            LoadPlayerKnownMonsters(progressDto.Player, _unitSchemeCatalog, Globe.Player);

            LoadAvailableLocations(progressDto.Player.AvailableLocations, Globe.Player);

            LoadMonsterPerkPool(progressDto.Player.MonsterPerks, Globe.Player);
        }
    }

    public void StoreCurrentGlobe()
    {
        if (_gameSettings.Mode == GameMode.Demo)
        {
            // Do not create save-files in demo version.
            // It will need when demo be longer that 1 hour gameplay (whole episode 1 developed).
            return;
        }

        var player = new PlayerDto
        {
            Heroes = CreateHeroesStorageData(Globe.Player.Heroes),
            Resources = GetPlayerResourcesToSave(Globe.Player.Inventory),
            KnownMonsterSids = GetKnownMonsterSids(Globe.Player.KnownMonsters),
            Abilities = Globe.Player.Abilities.Select(x => x.ToString()).ToArray(),
            AvailableLocations = Globe.Player.CurrentAvailableLocations.Select(x => x.ToString()).ToArray(),
            MonsterPerks = Globe.Player.MonsterPerks.Select(x => x.Sid).ToArray()
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

    private static HeroDto[] CreateHeroesStorageData(IEnumerable<HeroState> units)
    {
        var heroesStorageItems = units.Select(
            unit => new HeroDto
            {
                HeroSid = unit.ClassSid,
                Hp = unit.HitPoints.Current
            });

        return heroesStorageItems.ToArray();
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

    private static string[] GetKnownMonsterSids(IList<MonsterKnowledge> knownMonsters)
    {
        return knownMonsters.Select(x => x.ClassSid).ToArray();
    }

    private static ResourceDto[] GetPlayerResourcesToSave(Inventory inventory)
    {
        return inventory.CalcActualItems().Select(x => new ResourceDto
        {
            Amount = ((Resource)x).Count,
            Type = x.Scheme.Sid
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

    private static void InitStartStoryPoint(Globe globe, IStoryPointInitializer storyPointCatalog)
    {
        storyPointCatalog.Init(globe);
    }

    private static bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }

    private static void LoadAvailableLocations(string?[]? availableLocations, Player player)
    {
        if (availableLocations is null)
        {
            return;
        }

        foreach (var storedLocationSid in availableLocations)
        {
            if (storedLocationSid is null)
            {
                continue;
            }

            var locationSid = LocationHelper.ParseLocationFromCatalog(storedLocationSid);

            if (locationSid is null)
            {
                //TODO Log error and try to migrate save data

                continue;
            }

            player.AddLocation(locationSid);
        }
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

    private void LoadHeroes(PlayerDto lastSavePlayer)
    {
        if (lastSavePlayer.Heroes is null)
        {
            throw new InvalidOperationException();
        }

        var loadedHeroes = LoadUnlockedHeroes(lastSavePlayer.Heroes);

        foreach (var unit in loadedHeroes)
        {
            Globe.Player.AddHero(unit);
        }
    }

    private void LoadMonsterPerkPool(string?[]? monsterPerks, Player player)
    {
        if (monsterPerks is null)
        {
            return;
        }

        var allPerks = _monsterPerkCatalog.Perks;

        foreach (var perkSid in monsterPerks)
        {
            if (perkSid is null)
            {
                continue;
            }

            var perk = allPerks.SingleOrDefault(x => x.Sid == perkSid);

            if (perk is null)
            {
                //TODO Log unknown perk
                continue;
            }

            player.AddMonsterPerk(perk);
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

    private static void LoadPlayerKnownMonsters(PlayerDto playerDto, ICharacterCatalog unitSchemeCatalog,
        Player player)
    {
        player.KnownMonsters.Clear();

        if (playerDto.KnownMonsterSids is null)
        {
            return;
        }

        foreach (var monsterSid in playerDto.KnownMonsterSids)
        {
            if (monsterSid is null)
            {
                continue;
            }

            var monsterScheme = unitSchemeCatalog.AllMonsters.SingleOrDefault(x => x.Name.ToString() == monsterSid);

            if (monsterScheme is null)
            {
                Debug.Fail("Make migration of the save");
            }
            else
            {
                player.KnownMonsters.Add(new MonsterKnowledge(monsterSid, MonsterKnowledgeLevel.CommonDescription));
            }
        }
    }

    private static void LoadPlayerResources(ResourceDto?[]? resources, Inventory inventory)
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

            var resource = inventory.CalcActualItems().OfType<Resource>()
                .SingleOrDefault(x => x.Scheme.Sid == resourceDto.Type);
            resource ??= new Resource(new PropScheme(resourceDto.Type), resourceDto.Amount);

            inventory.Add(resource);
        }
    }

    private static List<HeroState> LoadUnlockedHeroes(HeroDto[] heroesStorageDataItems)
    {
        var units = new List<HeroState>();
        foreach (var heroDto in heroesStorageDataItems)
        {
            if (heroDto.HeroSid is null)
            {
                throw new InvalidOperationException($"Hero {heroDto.HeroSid} is unknown in save.");
            }

            var hero = HeroState.Create(heroDto.HeroSid);

            var hpDiff = hero.HitPoints.Current - heroDto.Hp;
            hero.HitPoints.Consume(hpDiff);

            units.Add(hero);
        }

        return units;
    }

    public sealed class SaveShortInfo
    {
        public string FileName { get; set; } = null!;

        [JsonPropertyName(nameof(SaveDto.Name))]
        public string PlayerName { get; init; } = null!;

        public DateTime UpdateTime { get; init; }
    }

    internal class SaveDto
    {
        public string Name { get; init; } = null!;
        public ProgressDto Progress { get; init; } = null!;
        public DateTime UpdateTime { get; init; }
    }
}