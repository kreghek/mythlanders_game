﻿using System.Collections.Generic;
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
        private Texture2D _biomClouds;
        private IDictionary<CombatBackgroundObjectTextureType, Texture2D> _combatBackgroundAnimatedObjectsTextureDict;
        private Dictionary<BackgroundType, Texture2D[]> _combatBackgroundDict;
        private Texture2D _combatUnitMarkers;
        private Dictionary<UnitName, SoundEffect> _deathSoundDict;
        private Texture2D _equipmentIcons;
        private SpriteFont _font;
        private IDictionary<UnitName, Texture2D> _heroFaceTextureDict;
        private Texture2D _locationObjectTextures;
        private Dictionary<GlobeNodeSid, Texture2D> _locationTextureDict;

        private Texture2D? _mapNodes;
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
            _biomClouds = contentManager.Load<Texture2D>("Sprites/GameObjects/Clouds");

            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _allWhiteEffect = contentManager.Load<Effect>("Effects/AllWhite");
            _playerUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.Berimir, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/Warrior") },
                { UnitName.Rada, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/Herbalist") },
                { UnitName.Hawk, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/Archer") },
                { UnitName.Maosin, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/Monk") }
            };

            _monsterUnitTextureDict = new Dictionary<UnitName, Texture2D>
            {
                { UnitName.GreyWolf, LoadMonsterTexture(contentManager, "Wolf") },
                { UnitName.Aspid, LoadMonsterTexture(contentManager, "Aspid") },
                { UnitName.Wisp, LoadMonsterTexture(contentManager, "Wisp") },
                { UnitName.Bear, LoadMonsterTexture(contentManager, "Bear") },
                { UnitName.VolkolakWarrior, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Volkolak, LoadMonsterTexture(contentManager, "Volkolak") },
                { UnitName.Stryga, LoadMonsterTexture(contentManager, "Stryga") }
            };

            _combatBackgroundDict = new Dictionary<BackgroundType, Texture2D[]>
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
                }
            };

            _skillSoundDict = new Dictionary<GameObjectSoundType, SoundEffect>
            {
                {
                    GameObjectSoundType.SwordSlash,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/SwordHitEffect")
                },
                {
                    GameObjectSoundType.Defence,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/ShieldEffect3")
                },
                {
                    GameObjectSoundType.EnergoShot,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/BowStrikeEffect")
                },
                {
                    GameObjectSoundType.Heal, contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/HealEffect")
                },
                {
                    GameObjectSoundType.StaffHit,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/StaffHitEffect")
                },
                {
                    GameObjectSoundType.MagicDust,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/DustEffect")
                },
                {
                    GameObjectSoundType.EgyptianDarkMagic,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/EgyptMassStunEffect")
                },

                {
                    GameObjectSoundType.WolfBite,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/WolfHitEffect")
                },
                {
                    GameObjectSoundType.AspidBite,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/SnakeHitEffect")
                },
                {
                    GameObjectSoundType.BearBludgeon,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/BearBludgeon")
                },
                {
                    GameObjectSoundType.WispEnergy,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/WispStrikeEffect")
                },
                {
                    GameObjectSoundType.VampireBite,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/VampireHitEffect")
                },

                {
                    GameObjectSoundType.SvarogSymbolAppearing,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/Lasers")
                },
                {
                    GameObjectSoundType.RisingPower,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/Shake")
                },
                {
                    GameObjectSoundType.Firestorm,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/Explosion")
                },
                {
                    GameObjectSoundType.FireDamage,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/FireDamage")
                }
            };

            _deathSoundDict = new Dictionary<UnitName, SoundEffect>
            {
                { UnitName.Berimir, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Hawk, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Rada, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Maosin, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Ping, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Cheng, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Amun, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Kakhotep, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Nubiti, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

                { UnitName.Leonidas, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Diana, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },
                { UnitName.Geron, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/HumanDeath") },

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

            _locationTextureDict = new Dictionary<GlobeNodeSid, Texture2D>
            {
                { GlobeNodeSid.Thicket, contentManager.Load<Texture2D>("Sprites/GameObjects/Map/DeepThicket") }
            };

            _locationObjectTextures = contentManager.Load<Texture2D>("Sprites/GameObjects/Map/MapObjects");

            _particlesTexture = contentManager.Load<Texture2D>("Sprites/GameObjects/SfxObjects/Particles");

            _combatBackgroundAnimatedObjectsTextureDict = new Dictionary<CombatBackgroundObjectTextureType, Texture2D>
            {
                {
                    CombatBackgroundObjectTextureType.Clouds,
                    contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/AnimatedObjects/Clouds")
                },
                {
                    CombatBackgroundObjectTextureType.Banner,
                    contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/AnimatedObjects/Banner")
                }
            };

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
                { UnitName.Undefined, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/SwordsmanFace") },
                { UnitName.Berimir, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/SwordsmanFace") },
                { UnitName.Rada, contentManager.Load<Texture2D>("Sprites/GameObjects/PlayerUnits/HerbalistFace") }
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
                    LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Far),
                    LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Main),
                    LoadBackgroundLayer(biomeType, locationSid, BackgroundLayerType.Closest)
                };
            }
        }

        internal Texture2D GetBiomeClouds()
        {
            return _biomClouds;
        }

        internal Texture2D GetBulletGraphics()
        {
            return _arrowTexture;
        }

        internal Texture2D GetCombatBackgroundAnimatedObjectsTexture(CombatBackgroundObjectTextureType textureType)
        {
            return _combatBackgroundAnimatedObjectsTextureDict[textureType];
        }

        internal Texture2D[] GetCombatBackgrounds(BackgroundType backgroundType)
        {
            return _combatBackgroundDict[backgroundType];
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

        internal Texture2D GetLocationObjectTextures()
        {
            return _locationObjectTextures;
        }

        internal Texture2D GetLocationTextures(GlobeNodeSid globeNodeSid)
        {
            return _locationTextureDict[GlobeNodeSid.Thicket];
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

        private static Texture2D LoadMonsterTexture(ContentManager contentManager, string spriteName)
        {
            var path = Path.Combine("Sprites", "GameObjects", "MonsterUnits", spriteName);
            return contentManager.Load<Texture2D>(path);
        }

        private enum BackgroundLayerType
        {
            Clouds,
            Far,
            Main,
            Closest
        }
    }
}