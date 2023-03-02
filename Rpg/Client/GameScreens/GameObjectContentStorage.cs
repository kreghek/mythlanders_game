using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal class GameObjectContentStorage
    {
        private Effect _allWhiteEffect;
        private Texture2D _arrowTexture;

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
        private Texture2D _puzzleTexture;
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

            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _allWhiteEffect = contentManager.Load<Effect>("Effects/AllWhite");
            _playerUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Comissar, LoadHeroTexture(contentManager, "Comissar") },
                { UnitName.Assaulter, LoadHeroTexture(contentManager, "Assaulter") },
                { UnitName.Swordsman, LoadHeroTexture(contentManager, "Swordsman") },
                { UnitName.Herbalist, LoadHeroTexture(contentManager, "Herbalist") },
                { UnitName.Archer, LoadHeroTexture(contentManager, "Archer") },
                { UnitName.Monk, LoadHeroTexture(contentManager, "Monk") },
                { UnitName.Spearman, LoadHeroTexture(contentManager, "Spearman") },
                { UnitName.Medjay, LoadHeroTexture(contentManager, "Medjay") },
                { UnitName.Hoplite, LoadHeroTexture(contentManager, "Hoplite") },
                { UnitName.Amazon, LoadHeroTexture(contentManager, "Amazon") }
            };

            LoadMonsters(contentManager);

            _combatBackgroundBaseDict = new Dictionary<BackgroundType, Texture2D[]>
            {
                {
                    BackgroundType.SlavicDarkThicket, LoadBackgroundLayers(BiomeType.Slavic, LocationSid.Thicket)
                },

                {
                    BackgroundType.SlavicBattleground, LoadBackgroundLayers(BiomeType.Slavic, LocationSid.Battleground)
                },

                {
                    BackgroundType.SlavicSwamp, LoadBackgroundLayers(BiomeType.Slavic, LocationSid.Swamp)
                },

                {
                    BackgroundType.SlavicDestroyedVillage,
                    LoadBackgroundLayers(BiomeType.Slavic, LocationSid.DestroyedVillage)
                },

                {
                    BackgroundType.ChineseMonastery, LoadBackgroundLayers(BiomeType.Chinese, LocationSid.Monastery)
                },

                {
                    BackgroundType.EgyptianDesert, LoadBackgroundLayers(BiomeType.Egyptian, LocationSid.Desert)
                },
                {
                    BackgroundType.EgyptianPyramids, LoadBackgroundLayers(BiomeType.Egyptian, LocationSid.SacredPlace)
                },

                {
                    BackgroundType.GreekShipGraveyard, LoadBackgroundLayers(BiomeType.Greek, LocationSid.ShipGraveyard)
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
                },

                {
                    new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Main256x256_0")
                },

                {
                    new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Far, 0),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Far256x256_0")
                },

                {
                    new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Main, 1),
                    contentManager.Load<Texture2D>(
                        "Sprites/GameObjects/CombatBackgrounds/Greek/ShipGraveyard/Main64x64_0")
                },

                {
                    new(BackgroundType.GreekShipGraveyard, BackgroundLayerType.Far, 2),
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
                { GameObjectSoundType.SwordSlash, LoadSkillEffect("SwordHitEffect") },
                { GameObjectSoundType.Defence, LoadSkillEffect("ShieldEffect3") },
                { GameObjectSoundType.EnergoShot, LoadSkillEffect("BowStrikeEffect") },
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
                { GameObjectSoundType.AmazonWarCry, LoadSkillEffect("AmazonWarCry") }
            };

            _deathSoundDict = new Dictionary<UnitName, SoundEffect>
            {
                { UnitName.Comissar, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Assaulter, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Swordsman, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/BerimirDeath") },
                { UnitName.Archer, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Herbalist, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },

                { UnitName.Monk, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Spearman, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Sage, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Medjay, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Priest, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Liberator, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },

                { UnitName.Hoplite, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Amazon, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/RadaDeath") },
                { UnitName.Engineer, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Marauder, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.BoldMarauder, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.BlackTrooper, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.DigitalWolf, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") },
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
                { UnitName.Swordsman, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Berimir") },
                { UnitName.Hq, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hq") },
                { UnitName.Archer, contentManager.Load<SoundEffect>("Audio/GameObjects/Text/Hawk") }
            };

            _heroFaceTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Hq, LoadHeroPortrait("Hq") },
                { UnitName.Undefined, LoadHeroPortrait("Undefined") },
                { UnitName.Swordsman, LoadHeroPortrait("Swordsman") },
                { UnitName.Archer, LoadHeroPortrait("Archer") },
                { UnitName.Herbalist, LoadHeroPortrait("Herbalist") },
                { UnitName.Assaulter, LoadHeroPortrait("Assaulter") },
                { UnitName.Monk, LoadHeroPortrait("Monk") },
                { UnitName.Spearman, LoadHeroPortrait("Spearman") },
                { UnitName.Hoplite, LoadHeroPortrait("Hoplite") },
                { UnitName.Synth, LoadHeroPortrait("DamagedSynth") }
            };

            Texture2D LoadBackgroundLayer(BiomeType biomeType, LocationSid locationSid, BackgroundLayerType layerType)
            {
                var imagePath = Path.Combine("Sprites", "GameObjects", "CombatBackgrounds", biomeType.ToString(),
                    locationSid.ToString(), $"{layerType}Layer");
                return contentManager.Load<Texture2D>(imagePath);
            }

            Texture2D[] LoadBackgroundLayers(BiomeType biomeType, LocationSid locationSid)
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

            _puzzleTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/Puzzle");
        }

        internal Texture2D GetBulletGraphics()
        {
            return _arrowTexture;
        }

        internal Texture2D GetCombatBackgroundObjectsTexture(BackgroundType backgroundType,
            BackgroundLayerType layerType, int spriteSheetIndex)
        {
            return _combatBackgroundObjectsDict[new(backgroundType, layerType, spriteSheetIndex)];
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

        private void LoadMonsters(ContentManager contentManager)
        {
            _monsterUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Marauder, LoadMonsterTexture(contentManager, "Marauder") },
                { UnitName.BoldMarauder, LoadMonsterTexture(contentManager, "BoldMarauder") },
                { UnitName.BlackTrooper, LoadMonsterTexture(contentManager, "BlackTrooper") },
                { UnitName.DigitalWolf, LoadMonsterTexture(contentManager, "DigitalWolf") },
                { UnitName.Aspid, LoadMonsterTexture(contentManager, "Aspid") },
                { UnitName.Wisp, LoadMonsterTexture(contentManager, "Wisp") },
                { UnitName.Bear, LoadMonsterTexture(contentManager, "Bear") },
                { UnitName.VolkolakWarrior, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Volkolak, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Stryga, LoadMonsterTexture(contentManager, "Stryga") },
                { UnitName.HornedFrog, LoadMonsterTexture(contentManager, "HornedFrog") },

                { UnitName.Huapigui, LoadMonsterTexture(contentManager, "Huapigui") },

                { UnitName.MummyWarrior, LoadMonsterTexture(contentManager, "Mummy") }
            };
        }

        private static Texture2D LoadMonsterTexture(ContentManager contentManager, string spriteName)
        {
            var path = Path.Combine("Sprites", "GameObjects", "MonsterUnits", spriteName);
            return contentManager.Load<Texture2D>(path);
        }
    }
}