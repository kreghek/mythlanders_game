using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.Models
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
        private SpriteFont _font;
        private Texture2D _locationObjectTextures;
        private Dictionary<GlobeNodeSid, Texture2D> _locationTextureDict;
        private Texture2D? _mapNodes;
        private Texture2D? _monsterUnit;
        private IDictionary<UnitName, Texture2D> _monsterUnitTextureDict;
        private Texture2D _particlesTexture;
        private IDictionary<UnitName, Texture2D> _playerUnitTextureDict;
        private Texture2D _shadowTexture;

        private Dictionary<GameObjectSoundType, SoundEffect> _skillSoundDict;
        private Texture2D _unitPortrains;

        public Effect GetAllWhiteEffect()
        {
            return _allWhiteEffect;
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
                { UnitName.Aspid, contentManager.Load<Texture2D>("Sprites/GameObjects/MonsterUnits/Aspid") }
            };

            _combatBackgroundDict = new Dictionary<BackgroundType, Texture2D[]>
            {
                {
                    BackgroundType.SlavicDarkThicket, new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Thicket/CloudsLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Thicket/FarLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Thicket/MainLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Thicket/ClosestLayer")
                    }
                },

                {
                    BackgroundType.SlavicBattleground, new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/CloudsLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/FarLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/MainLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Battleground/ClosestLayer")
                    }
                },

                {
                    BackgroundType.SlavicSwamp, new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/CloudsLayer"),
                        contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/FarLayer"),
                        contentManager.Load<Texture2D>("Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/MainLayer"),
                        contentManager.Load<Texture2D>(
                            "Sprites/GameObjects/CombatBackgrounds/Slavic/Swamp/ClosestLayer")
                    }
                }
            };

            _skillSoundDict = new Dictionary<GameObjectSoundType, SoundEffect>
            {
                {
                    GameObjectSoundType.SwordSlash,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/SwordHitEffect")
                },
                {
                    GameObjectSoundType.BowShot,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/BowStrikeEffect")
                },
                { GameObjectSoundType.Heal, contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/HealEffect") },
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
                    GameObjectSoundType.SnakeBite,
                    contentManager.Load<SoundEffect>("Audio/GameObjects/Skills/SnakeHitEffect")
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

                { UnitName.GreyWolf, contentManager.Load<SoundEffect>("Audio/GameObjects/Deaths/DogDeath") }
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
                }
                ;
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

        internal Texture2D GetUnitPortrains()
        {
            return _unitPortrains;
        }

        internal Texture2D GetUnitShadow()
        {
            return _shadowTexture;
        }
    }
}