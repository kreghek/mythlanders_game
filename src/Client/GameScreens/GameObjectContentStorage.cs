using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Client.Assets;
using Client.Assets.CombatMovements;
using Client.Core;
using Client.Core.Heroes.Factories;
using Client.Engine;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json;

namespace Client.GameScreens;

internal class GameObjectContentStorage
{
    private Effect _allWhiteEffect;

    private IDictionary<string, SpriteAtlasAnimationData>? _animationSetDict;
    private Texture2D _arrowTexture;

    private Texture2D _combatantMarkers;

    private IDictionary<LocationTheme, Texture2D[]> _combatBackgroundBaseDict;
    private IDictionary<(LocationTheme, BackgroundLayerType, int), Texture2D> _combatBackgroundObjectsDict;

    private ContentManager? _contentManager;
    private IDictionary<UnitName, SoundEffect> _deathSoundDict;
    private Texture2D _equipmentIcons;
    private SpriteFont _font;
    private IDictionary<UnitName, Texture2D> _heroPortraitsTextureDict;
    private IDictionary<UnitName, Texture2D> _heroTextureDict;

    private Texture2D? _mapNodes;
    private Texture2D _mapTexture;
    private IDictionary<UnitName, Texture2D> _monsterUnitTextureDict;
    private Texture2D _particlesTexture;
    private Texture2D _puzzleTexture;
    private Texture2D _shadowTexture;

    private Dictionary<GameObjectSoundType, SoundEffect> _skillSoundDict;
    private Texture2D _svarogSymbolTexture;

    private IDictionary<UnitName, SoundEffect> _textSoundDict;

    public Effect GetAllWhiteEffect()
    {
        return _allWhiteEffect;
    }

    public Texture2D GetCharacterFaceTexture(UnitName heroSid)
    {
        if (_heroPortraitsTextureDict.TryGetValue(heroSid, out var texture))
        {
            return texture;
        }

        return _heroPortraitsTextureDict[UnitName.Undefined];
    }

    public Texture2D GetSymbolSprite()
    {
        return _svarogSymbolTexture;
    }

    public Texture2D GetUnitGraphics(UnitName unitName)
    {
        if (_heroTextureDict.TryGetValue(unitName, out var playerUnitTexture))
        {
            return playerUnitTexture;
        }

        if (_monsterUnitTextureDict.TryGetValue(unitName, out var monsterUnitTexture))
        {
            return monsterUnitTexture;
        }

        throw new InvalidOperationException();
    }

    public void LoadContent(ContentManager contentManager)
    {
        _contentManager = contentManager;

        _mapNodes = contentManager.Load<Texture2D>("Sprites/GameObjects/MapNodes");
        _combatantMarkers = contentManager.Load<Texture2D>("Sprites/GameObjects/CombatantMarkers");

        _font = contentManager.Load<SpriteFont>("Fonts/Main");

        _allWhiteEffect = contentManager.Load<Effect>("Effects/AllWhite");

        var heroFactories = LoadHeroFactories();

        _heroTextureDict = heroFactories.ToDictionary(x => GetUnitNameByHerofactoryName(x),
            x => LoadHeroesTexture(contentManager, x.ClassSid));

        LoadMonsters(contentManager);

        _combatBackgroundBaseDict = new Dictionary<LocationTheme, Texture2D[]>
        {
            {
                LocationTheme.SlavicDarkThicket, LoadBackgroundLayers(LocationCulture.Slavic, LocationSids.Thicket)
            },

            {
                LocationTheme.SlavicBattleground,
                LoadBackgroundLayers(LocationCulture.Slavic, LocationSids.Battleground)
            },

            {
                LocationTheme.SlavicSwamp, LoadBackgroundLayers(LocationCulture.Slavic, LocationSids.Swamp)
            },

            {
                LocationTheme.SlavicDestroyedVillage,
                LoadBackgroundLayers(LocationCulture.Slavic, LocationSids.DestroyedVillage)
            },

            {
                LocationTheme.ChineseMonastery, LoadBackgroundLayers(LocationCulture.Chinese, LocationSids.Monastery)
            },

            {
                LocationTheme.EgyptianDesert, LoadBackgroundLayers(LocationCulture.Egyptian, LocationSids.Desert)
            },
            {
                LocationTheme.EgyptianSacredPlace,
                LoadBackgroundLayers(LocationCulture.Egyptian, LocationSids.SacredPlace)
            },

            {
                LocationTheme.GreekShipGraveyard,
                LoadBackgroundLayers(LocationCulture.Greek, LocationSids.ShipGraveyard)
            }
        };

        _combatBackgroundObjectsDict = new Dictionary<(LocationTheme, BackgroundLayerType, int), Texture2D>
        {
            {
                new(LocationTheme.SlavicBattleground, BackgroundLayerType.Closest, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/Closest256x256_0")
            },

            {
                new(LocationTheme.SlavicBattleground, BackgroundLayerType.Clouds, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/Clouds256x256_0")
            },

            {
                new(LocationTheme.ChineseMonastery, BackgroundLayerType.Main, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Chinese/Monastery/Main256x256_0")
            },

            {
                new(LocationTheme.ChineseMonastery, BackgroundLayerType.Far, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Chinese/Monastery/Far256x256_0")
            },

            {
                new(LocationTheme.GreekShipGraveyard, BackgroundLayerType.Main, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Main256x256_0")
            },

            {
                new(LocationTheme.GreekShipGraveyard, BackgroundLayerType.Far, 0),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Far256x256_0")
            },

            {
                new(LocationTheme.GreekShipGraveyard, BackgroundLayerType.Main, 1),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Main64x64_0")
            },

            {
                new(LocationTheme.GreekShipGraveyard, BackgroundLayerType.Far, 2),
                contentManager.Load<Texture2D>(
                    "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Far64x64_0")
            }
        };

        SoundEffect LoadSkillEffect(string name)
        {
            return contentManager.Load<SoundEffect>($"Audio/GameObjects/Skills/{name}");
        }

        _skillSoundDict = new Dictionary<GameObjectSoundType, SoundEffect>
        {
            { GameObjectSoundType.SwordPrepare, LoadSkillEffect("PrepareSword") },
            { GameObjectSoundType.SwordSlash, LoadSkillEffect("SwordHitEffect") },
            { GameObjectSoundType.ArmedMove, LoadSkillEffect("ArmedMove") },
            { GameObjectSoundType.Defence, LoadSkillEffect("ShieldEffect3") },
            { GameObjectSoundType.ImpulseBowShot, LoadSkillEffect("BowStrikeEffect") },
            { GameObjectSoundType.Heal, LoadSkillEffect("HealEffect") },
            { GameObjectSoundType.StaffHit, LoadSkillEffect("StaffHitEffect") },
            { GameObjectSoundType.MagicDust, LoadSkillEffect("DustEffect") },
            { GameObjectSoundType.EgyptianDarkMagic, LoadSkillEffect("EgyptMassStunEffect") },
            { GameObjectSoundType.DigitalBite, LoadSkillEffect("WolfHitEffect") },
            { GameObjectSoundType.AspidBite, LoadSkillEffect("SnakeHitEffect") },
            { GameObjectSoundType.BearBludgeon, LoadSkillEffect("BearBludgeon") },
            { GameObjectSoundType.WispEnergy, LoadSkillEffect("WispStrikeEffect") },
            { GameObjectSoundType.VampireBite, LoadSkillEffect("VampireHitEffect") },
            { GameObjectSoundType.SvarogSymbolAppearing, LoadSkillEffect("Lasers") },
            { GameObjectSoundType.RisingPower, LoadSkillEffect("Shake") },
            { GameObjectSoundType.Firestorm, LoadSkillEffect("Explosion") },
            { GameObjectSoundType.FireDamage, LoadSkillEffect("FireDamage") },
            { GameObjectSoundType.FrogHornsUp, LoadSkillEffect("FrogHornsUp") },
            { GameObjectSoundType.Gunshot, LoadSkillEffect("Gunshot") },
            { GameObjectSoundType.AssaultRifleBurst, LoadSkillEffect("AssaultRifleBurst") },
            { GameObjectSoundType.CyberRifleShot, LoadSkillEffect("CyberRifleShot") },
            { GameObjectSoundType.AmazonWarCry, LoadSkillEffect("AmazonWarCry") },
            { GameObjectSoundType.ImpulseArrowBlasts, LoadSkillEffect("RainingArrowBlasts") }
        };

        _deathSoundDict = new Dictionary<UnitName, SoundEffect>
        {
            { UnitName.Partisan, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.ShieldBearer, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

            { UnitName.Swordsman, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/BerimirDeath") },
            { UnitName.Robber, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Herbalist, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },

            { UnitName.Monk, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Guardian, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Sage, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

            { UnitName.Medjay, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Priest, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Liberator, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },

            { UnitName.Hoplite, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Amazon, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },
            { UnitName.Engineer, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

            { UnitName.Marauder, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.BoldMarauder, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
            { UnitName.Agressor, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

            { UnitName.DigitalWolf, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") },
            { UnitName.CorruptedBear, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/BearDeath") },
            { UnitName.Wisp, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/WhispDeath") },
            { UnitName.Aspid, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/AspidDeath") },
            {
                UnitName.VolkolakWarrior,
                contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/WolfWarriorShapeShift")
            },
            { UnitName.Volkolak, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") },
            { UnitName.AmbushDrone, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/AmbushDrone") }
        };

        _shadowTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SimpleObjectShadow");

        _arrowTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Arrow");

        _mapTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/Map/Map");

        _particlesTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Particles");

        _svarogSymbolTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/SvarogFireSfx");
        _equipmentIcons = contentManager.Load<Texture2D>("Sprites/GameObjects/EquipmentIcons");

        _textSoundDict = new Dictionary<UnitName, SoundEffect>
        {
            { UnitName.Environment, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Environment") },
            { UnitName.Swordsman, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Berimir") },
            { UnitName.Hq, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hq") },
            { UnitName.Robber, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hawk") }
        };

        _heroPortraitsTextureDict =
            heroFactories.ToDictionary(x => GetUnitNameByHerofactoryName(x), x => LoadHeroPortrait(x.ClassSid));

        //NPCs
        _heroPortraitsTextureDict.Add(UnitName.Synth, LoadNpcPortrait("DamagedSynth"));
        _heroPortraitsTextureDict.Add(UnitName.ChineseOldman, LoadNpcPortrait("ChineseOldman"));
        _heroPortraitsTextureDict.Add(UnitName.Undefined, LoadUndefinedPortrait());

        Texture2D LoadBackgroundLayer(LocationCulture biomeType, ILocationSid locationSid,
            BackgroundLayerType layerType)
        {
            var imagePath = Path.Combine("Sprites", "GameObjects", "CombatBackgrounds", biomeType.ToString(),
                locationSid.ToString(), $"{layerType}Layer");
            return contentManager.Load<Texture2D>(imagePath);
        }

        Texture2D[] LoadBackgroundLayers(LocationCulture biomeType, ILocationSid locationSid)
        {
            return new[]
            {
                LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Clouds),
                LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Semi),
                LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Far),
                LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Main),
                LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Closest)
            };
        }

        Texture2D LoadHeroPortrait(string classSid)
        {
            return contentManager.Load<Texture2D>(Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes",
                classSid, "Portrait"));
        }

        Texture2D LoadNpcPortrait(string classSid)
        {
            return contentManager.Load<Texture2D>(Path.Combine(CommonConstants.PathToCharacterSprites, "Npc",
                classSid, "Portrait"));
        }

        Texture2D LoadUndefinedPortrait()
        {
            return contentManager.Load<Texture2D>(Path.Combine(CommonConstants.PathToCharacterSprites, "Undefined",
                "Portrait"));
        }

        _puzzleTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/Puzzle");

        _animationSetDict = new Dictionary<string, SpriteAtlasAnimationData>
        {
            { "Swordsman", GetAnimationInner("Swordsman") },
            { "Partisan", GetAnimationInner("Partisan") },
            { "Robber", GetAnimationInner("Robber") },

            { "DigitalWolf", GetAnimationInner("DigitalWolf") },
            { "Aspid", GetAnimationInner("Aspid") },

            { "AmbushDrone", GetAnimationInner("AmbushDrone") }
        };
    }

    internal SpriteAtlasAnimationData GetAnimation(string animationSet)
    {
        if (_animationSetDict is null)
        {
            throw new InvalidOperationException("Storage is not loaded.");
        }

        return _animationSetDict[animationSet];
    }

    internal Texture2D GetBulletGraphics()
    {
        return _arrowTexture;
    }

    internal Texture2D GetCombatantMarkers()
    {
        return _combatantMarkers;
    }

    internal Texture2D GetCombatBackgroundObjectsTexture(LocationTheme backgroundType,
        BackgroundLayerType layerType, int spriteSheetIndex)
    {
        return _combatBackgroundObjectsDict[new(backgroundType, layerType, spriteSheetIndex)];
    }

    internal Texture2D[] GetCombatBackgrounds(LocationTheme locationTheme)
    {
        return _combatBackgroundBaseDict[locationTheme];
    }

    internal SoundEffect GetDeathSound(UnitName unitName)
    {
        if (_deathSoundDict.TryGetValue(unitName, out var soundEffect))
        {
            return soundEffect;
        }

        return _deathSoundDict[UnitName.DigitalWolf];
    }

    internal Texture2D GetEquipmentIcons()
    {
        return _equipmentIcons;
    }

    internal SpriteFont GetFont()
    {
        return _font;
    }

    internal Texture2D GetMapTexture()
    {
        return _mapTexture;
    }

    internal Texture2D GetNodeMarker()
    {
        return _mapNodes;
    }

    internal Texture2D GetParticlesTexture()
    {
        return _particlesTexture;
    }

    internal Texture2D GetPuzzleTexture()
    {
        return _puzzleTexture;
    }

    internal SoundEffect GetSkillUsageSound(GameObjectSoundType soundType)
    {
        if (_skillSoundDict.TryGetValue(soundType, out var soundEffect))
        {
            return soundEffect;
        }

        return _skillSoundDict[GameObjectSoundType.DigitalBite];
    }

    internal SoundEffect GetTextSoundEffect(UnitName unitName)
    {
        if (!_textSoundDict.TryGetValue(unitName, out var soundEffect))
        {
            return _textSoundDict[UnitName.Swordsman];
        }

        return soundEffect;
    }

    internal Texture2D GetUnitShadow()
    {
        return _shadowTexture;
    }

    private SpriteAtlasAnimationData GetAnimationInner(string animationSet)
    {
        var contentManager = GetContentManager();
        var json = contentManager.Load<string>(Path.Combine("Animations", animationSet));
        return JsonConvert.DeserializeObject<SpriteAtlasAnimationData>(json);
    }

    private ContentManager GetContentManager()
    {
        return _contentManager ?? throw new InvalidOperationException("Storage is not loaded.");
    }

    private static UnitName GetUnitNameByHerofactoryName(IHeroFactory x)
    {
        return Enum.Parse<UnitName>(x.ClassSid);
    }

    private static Texture2D LoadHeroesTexture(ContentManager contentManager, string classSid)
    {
        var path = Path.Combine("Sprites", "GameObjects", "Characters", "Heroes", classSid, "Full");
        return contentManager.Load<Texture2D>(path);
    }

    private static IEnumerable<IHeroFactory> LoadHeroFactories()
    {
        var assembly = typeof(IHeroFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IHeroFactory).IsAssignableFrom(x) && x != typeof(IHeroFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IHeroFactory>().ToArray();
    }

    private void LoadMonsters(ContentManager contentManager)
    {
        _monsterUnitTextureDict = new Dictionary<UnitName, Texture2D>
        {
            { UnitName.Marauder, LoadMonsterTexture(contentManager, "Marauder") },
            { UnitName.BoldMarauder, LoadMonsterTexture(contentManager, "BoldMarauder") },
            { UnitName.Agressor, LoadMonsterTexture(contentManager, "Agressor") },
            { UnitName.AmbushDrone, LoadMonsterTexture(contentManager, "AmbushDrone") },
            { UnitName.DigitalWolf, LoadMonsterTexture(contentManager, "DigitalWolf") },
            { UnitName.Aspid, LoadMonsterTexture(contentManager, "Aspid") },
            { UnitName.Wisp, LoadMonsterTexture(contentManager, "Wisp") },
            { UnitName.CorruptedBear, LoadMonsterTexture(contentManager, "CorruptedBear") },
            { UnitName.VolkolakWarrior, LoadMonsterTexture(contentManager, "Volkolak") },
            { UnitName.Volkolak, LoadMonsterTexture(contentManager, "Volkolak") },
            { UnitName.Stryga, LoadMonsterTexture(contentManager, "Stryga") },
            { UnitName.HornedFrog, LoadMonsterTexture(contentManager, "HornedFrog") },

            { UnitName.Huapigui, LoadMonsterTexture(contentManager, "Huapigui") },
            { UnitName.PaintedSkin, LoadMonsterTexture(contentManager, "PaintedSkin") },

            { UnitName.Chaser, LoadMonsterTexture(contentManager, "Chaser") },

            { UnitName.Automataur, LoadMonsterTexture(contentManager, "Automataur") }
        };
    }

    private static Texture2D LoadMonsterTexture(ContentManager contentManager, string classSid)
    {
        var cultureSid = CharacterHelper.GetCultureSid(classSid);
        var path = Path.Combine("Sprites", "GameObjects", "Characters", "Monsters", cultureSid.ToString(), classSid,
            "Full");
        return contentManager.Load<Texture2D>(path);
    }
}