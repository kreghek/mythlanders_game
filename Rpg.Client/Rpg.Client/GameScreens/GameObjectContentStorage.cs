using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal enum BackgroundLayerType
    {
        Clouds, // Rename to horizon
        Semi, // Rename to Far
        Far, // Rename to Closest
        Main,
        Closest // Rename to foreground
    }

    internal class GameObjectContentStorage
    {
        private Effect _allWhiteEffect;
        private Texture2D _arrowTexture;
        private Texture2D _biomeClouds;

        private IDictionary<BackgroundType, Texture2D[]> _combatBackgroundBaseDict;
        private IDictionary<(BackgroundType, BackgroundLayerType, int), Texture2D> _combatBackgroundObjectsDict;

        private Texture2D _combatUnitMarkers;
        private IDictionary<UnitName, SoundEffect> _deathSoundDict;
        private Texture2D _equipmentIcons;
        private SpriteFont _font;
        private IDictionary<UnitName, Texture2D> _heroFaceTextureDict;

        private Texture2D? _mapNodes;
        private Texture2D _mapTexture;
        private Texture2D? _monsterUnit;
        private IDictionary<UnitName, Texture2D> _monsterUnitTextureDict;
        private Texture2D _particlesTexture;
        private IDictionary<UnitName, Texture2D> _playerUnitTextureDict;
        private Texture2D _shadowTexture;

        private Dictionary<GameObjectSoundType, SoundEffect> _skillSoundDict;
        private Texture2D _svarogSymbolTexture;

        private IDictionary<UnitName, SoundEffect> _textSoundDict;
        private Texture2D _unitPortrains;

        public Effect GetAllWhiteEffect()
        {
            return _allWhiteEffect;
        }

        public Texture2D GetCharacterFaceTexture(UnitName heroSid)
        {
            if (_heroFaceTextureDict.TryGetValue(heroSid, out var texture))
            {
                return texture;
            }

            return _heroFaceTextureDict[UnitName.Undefined];
        }

        public Texture2D GetSymbolSprite()
        {
            return _svarogSymbolTexture;
        }

        public Texture2D GetUnitGraphics(UnitName unitName)
        {
            if (_playerUnitTextureDict.TryGetValue(unitName, out var playerUnitTexture))
            {
                return playerUnitTexture;
            }

            if (_monsterUnitTextureDict.TryGetValue(unitName, out var monsterUnitTexture))
            {
                return monsterUnitTexture;
            }

            return _monsterUnit;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _monsterUnit = contentManager.Load<Texture2D>("Sprites/GameObjects/MonsterUnits/Wolf");
            _mapNodes = contentManager.Load<Texture2D>("Sprites/GameObjects/MapNodes");
            _combatUnitMarkers = contentManager.Load<Texture2D>("Sprites/GameObjects/CombatUnitMarkers");
            _biomeClouds = contentManager.Load<Texture2D>("Sprites/GameObjects/Clouds");

            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _allWhiteEffect = contentManager.Load<Effect>("Effects/AllWhite");
            _playerUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Thar, LoadHeroTexture(contentManager, "Sergant") },
                { UnitName.Dull, LoadHeroTexture(contentManager, "Assaulter") },
                { UnitName.Berimir, LoadHeroTexture(contentManager, "Warrior") },
                { UnitName.Rada, LoadHeroTexture(contentManager, "Herbalist") },
                { UnitName.Hawk, LoadHeroTexture(contentManager, "Archer") },
                { UnitName.Maosin, LoadHeroTexture(contentManager, "Monk") },
                { UnitName.Ping, LoadHeroTexture(contentManager, "Spearman") }
            };

            _monsterUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Marauder, LoadMonsterTexture(contentManager, "Marauder") },
                { UnitName.BlackTrooper, LoadMonsterTexture(contentManager, "BlackTrooper") },
                { UnitName.GreyWolf, LoadMonsterTexture(contentManager, "Wolf") },
                { UnitName.Aspid, LoadMonsterTexture(contentManager, "Aspid") },
                { UnitName.Wisp, LoadMonsterTexture(contentManager, "Wisp") },
                { UnitName.Bear, LoadMonsterTexture(contentManager, "Bear") },
                { UnitName.VolkolakWarrior, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Volkolak, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Stryga, LoadMonsterTexture(contentManager, "Stryga") },
                { UnitName.HornedFrog, LoadMonsterTexture(contentManager, "HornedFrog") },

                { UnitName.Mummy, LoadMonsterTexture(contentManager, "Mummy") }
            };

            _combatBackgroundBaseDict = new Dictionary<BackgroundType, Texture2D[]>
            {
                {
                    BackgroundType.SlavicDarkThicket, LoadBackgroundLayers(BiomeType.Slavic, GlobeNodeSid.Thicket)
                },

                {
                    BackgroundType.SlavicBattleground, LoadBackgroundLayers(BiomeType.Slavic, GlobeNodeSid.Battleground)
                },

                {
                    BackgroundType.SlavicSwamp, LoadBackgroundLayers(BiomeType.Slavic, GlobeNodeSid.Swamp)
                },

                {
                    BackgroundType.SlavicDestroyedVillage,
                    LoadBackgroundLayers(BiomeType.Slavic, GlobeNodeSid.DestroyedVillage)
                },

                {
                    BackgroundType.ChineseMonastery, LoadBackgroundLayers(BiomeType.Chinese, GlobeNodeSid.Monastery)
                },

                {
                    BackgroundType.EgyptianDisert, LoadBackgroundLayers(BiomeType.Egyptian, GlobeNodeSid.Disert)
                },
                {
                    BackgroundType.EgyptianPyramids, LoadBackgroundLayers(BiomeType.Egyptian, GlobeNodeSid.SacredPlace)
                }
            };

            _combatBackgroundObjectsDict = new Dictionary<(BackgroundType, BackgroundLayerType, int), Texture2D>
            {
                {
                    new(BackgroundType.SlavicBattleground, BackgroundLayerType.Closest, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/Closest256x256_0")
                },

                {
                    new(BackgroundType.SlavicBattleground, BackgroundLayerType.Clouds, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/Clouds256x256_0")
                },

                {
                    new(BackgroundType.ChineseMonastery, BackgroundLayerType.Main, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Chinese/Monastery/Main256x256_0")
                },

                {
                    new(BackgroundType.ChineseMonastery, BackgroundLayerType.Far, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Chinese/Monastery/Far256x256_0")
                }
            };

            SoundEffect LoadSkillEffect(string name)
            {
                return contentManager.Load<SoundEffect>($"Audio/GameObjects/Skills/{name}");
            }

            _skillSoundDict = new Dictionary<GameObjectSoundType, SoundEffect>
            {
                { GameObjectSoundType.SwordSlash, LoadSkillEffect("SwordHitEffect") },
                { GameObjectSoundType.Defence, LoadSkillEffect("ShieldEffect3") },
                { GameObjectSoundType.EnergoShot, LoadSkillEffect("BowStrikeEffect") },
                { GameObjectSoundType.Heal, LoadSkillEffect("HealEffect") },
                { GameObjectSoundType.StaffHit, LoadSkillEffect("StaffHitEffect") },
                { GameObjectSoundType.MagicDust, LoadSkillEffect("DustEffect") },
                { GameObjectSoundType.EgyptianDarkMagic, LoadSkillEffect("EgyptMassStunEffect") },
                { GameObjectSoundType.WolfBite, LoadSkillEffect("WolfHitEffect") },
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
                { GameObjectSoundType.AssaultRifleBurst, LoadSkillEffect("AssaultRifleBurst") }
            };

            _deathSoundDict = new Dictionary<UnitName, SoundEffect>
            {
                { UnitName.Thar, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Dull, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Berimir, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/BerimirDeath") },
                { UnitName.Hawk, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Rada, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },

                { UnitName.Maosin, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Ping, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Cheng, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Amun, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Kakhotep, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Nubiti, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Leonidas, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Diana, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Geron, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Marauder, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.BlackTrooper, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.GreyWolf, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") },
                { UnitName.Bear, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/BearDeath") },
                { UnitName.Wisp, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/WhispDeath") },
                { UnitName.Aspid, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/AspidDeath") },
                {
                    UnitName.VolkolakWarrior,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/WolfWarriorShapeShift")
                },
                { UnitName.Volkolak, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") }
            };

            _shadowTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SimpleObjectShadow");

            _unitPortrains = contentManager.Load<Texture2D>("Sprites/GameObjects/UnitPortrains");

            _arrowTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Arrow");

            _mapTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/Map/Map");

            _particlesTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Particles");

            _svarogSymbolTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/SvarogFireSfx");
            _equipmentIcons = contentManager.Load<Texture2D>("Sprites/GameObjects/EquipmentIcons");

            _textSoundDict = new Dictionary<UnitName, SoundEffect>
            {
                { UnitName.Environment, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Environment") },
                { UnitName.Berimir, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Berimir") },
                { UnitName.Hq, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hq") },
                { UnitName.Hawk, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hawk") }
            };

            _heroFaceTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Hq, LoadHeroPortrait("Hq") },
                { UnitName.Undefined, LoadHeroPortrait("Undefined") },
                { UnitName.Berimir, LoadHeroPortrait("Swordsman") },
                { UnitName.Hawk, LoadHeroPortrait("Archer") },
                { UnitName.Rada, LoadHeroPortrait("Herbalist") },
                { UnitName.Dull, LoadHeroPortrait("Assaulter") },
                { UnitName.Maosin, LoadHeroPortrait("Monk") }
            };

            Texture2D LoadBackgroundLayer(BiomeType biomeType, GlobeNodeSid locationSid, BackgroundLayerType layerType)
            {
                var imagePath = Path.Combine("Sprites", "GameObjects", "CombatBackgrounds", biomeType.ToString(),
                    locationSid.ToString(), $"{layerType}Layer");
                return contentManager.Load<Texture2D>(imagePath);
            }

            Texture2D[] LoadBackgroundLayers(BiomeType biomeType, GlobeNodeSid locationSid)
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

            Texture2D LoadHeroPortrait(string name)
            {
                return contentManager.Load<Texture2D>($"Sprites/GameObjects/PlayerUnits/{name}Face");
            }
        }

        internal Texture2D GetBiomeClouds()
        {
            return _biomeClouds;
        }

        internal Texture2D GetBulletGraphics()
        {
            return _arrowTexture;
        }

        internal Texture2D GetCombatBackgroundObjectsTexture(BackgroundType backgroundType,
            BackgroundLayerType layerType, int spritesheetIndex)
        {
            return _combatBackgroundObjectsDict[new(backgroundType, layerType, spritesheetIndex)];
        }

        internal Texture2D[] GetCombatBackgrounds(BackgroundType backgroundType)
        {
            return _combatBackgroundBaseDict[backgroundType];
        }

        internal Texture2D GetCombatUnitMarker()
        {
            return _combatUnitMarkers;
        }

        internal SoundEffect GetDeathSound(UnitName unitName)
        {
            if (_deathSoundDict.TryGetValue(unitName, out var soundEffect))
            {
                return soundEffect;
            }

            return _deathSoundDict[UnitName.GreyWolf];
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

        internal SoundEffect GetSkillUsageSound(GameObjectSoundType soundType)
        {
            if (_skillSoundDict.TryGetValue(soundType, out var soundEffect))
            {
                return soundEffect;
            }

            return _skillSoundDict[GameObjectSoundType.WolfBite];
        }

        internal SoundEffect GetTextSoundEffect(UnitName unitName)
        {
            if (!_textSoundDict.TryGetValue(unitName, out var soundEffect))
            {
                return _textSoundDict[UnitName.Berimir];
            }

            return soundEffect;
        }

        internal Texture2D GetUnitPortrains()
        {
            return _unitPortrains;
        }

        internal Texture2D GetUnitShadow()
        {
            return _shadowTexture;
        }

        private static Texture2D LoadHeroTexture(ContentManager contentManager, string spriteName)
        {
            var path = Path.Combine("Sprites", "GameObjects", "PlayerUnits", spriteName);
            return contentManager.Load<Texture2D>(path);
        }

        private static Texture2D LoadMonsterTexture(ContentManager contentManager, string spriteName)
        {
            var path = Path.Combine("Sprites", "GameObjects", "MonsterUnits", spriteName);
            return contentManager.Load<Texture2D>(path);
        }
    }
}