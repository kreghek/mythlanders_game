using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class UnitSchemeCatalog : IUnitSchemeCatalog
    {
        private static readonly UnitScheme SwordsmanHero = new()
        {
            TankRank = 0.4f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.1f,

            Name = UnitName.Berimir,
            
            Levels = new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new SwordSlashSkill()),
                new AddSkillUnitLevel(2, new WideSlashSkill()),
                new AddPerkUnitLevel(2, new ImprovedHitPoints()),
                new AddSkillUnitLevel(3, new DefenseStanceSkill(true)),
                new AddSkillUnitLevel(4, new SvarogBlastFurnaceSkill(true)),
            },
            
            Equipments = new IEquipmentScheme[]
            {
                new WarriorGreatSword(),
                new Mk2MediumPowerArmor()
            },
            
            UnitGraphicsConfig = new BerimirGraphicsConfig()
        };

        private static readonly UnitScheme MonkHero = new()
        {
            TankRank = 0.2f,
            DamageDealerRank = 0.6f,
            SupportRank = 0.2f,

            Name = UnitName.Maosin,
            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new StaffSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new StaffSkill(),
            //             new DefenseStanceSkill(true)
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new StaffSkill(),
            //             new DefenseStanceSkill(true),
            //             new WideSlashSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new MaosinGraphicsConfig()
        };

        private static readonly UnitScheme SpearmanHero = new()
        {
            TankRank = 0.8f,
            DamageDealerRank = 0.1f,
            SupportRank = 0.1f,

            Name = UnitName.Ping,
            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill(),
            //             new DefenseStanceSkill(true)
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill(),
            //             new DefenseStanceSkill(true),
            //             new WideSlashSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme ScorpionHero = new()
        {
            TankRank = 0.1f,
            DamageDealerRank = 0.8f,
            SupportRank = 0.1f,

            Name = UnitName.Amun,
            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill(),
            //             new DefenseStanceSkill(true)
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new SwordSlashSkill(),
            //             new DefenseStanceSkill(true),
            //             new WideSlashSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme HerbalistHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.0f,
            SupportRank = 1.0f,

            Name = UnitName.Rada,
            
            Levels = new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new HealingSalveSkill()),
                new AddSkillUnitLevel(2, new ToxicHerbsSkill()),
                new AddPerkUnitLevel(2, new CriticalHeal()),
                new AddSkillUnitLevel(3, new DopeHerbSkill(true)),
                new AddSkillUnitLevel(4, new MassHealSkill(true)),
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        private static readonly UnitScheme ArcherHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.25f,

            Name = UnitName.Hawk,
            
            Levels = new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new EnergyShotSkill()),
                new AddSkillUnitLevel(2, new RapidBowShotSkill()),
                new AddPerkUnitLevel(2, new CriticalHit()),
                new AddSkillUnitLevel(3, new ArrowRainSkill(true)),
                new AddSkillUnitLevel(4, new DefenseStanceSkill(true)),
            },
            
            Equipments = new IEquipmentScheme[]
            {
                new ArcherPulsarBow(),
                new Mk3ScoutPowerArmor()
            },
            
            UnitGraphicsConfig = new HawkGraphicsConfig()
        };

        private static readonly UnitScheme PriestHero = new()
        {
            TankRank = 0.1f,
            DamageDealerRank = 0.9f,
            SupportRank = 0.0f,

            Name = UnitName.Kakhotep,

            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new BowShotSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new BowShotSkill(),
            //             new MassStunSkill(true)
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new BowShotSkill(),
            //             new MassStunSkill(true),
            //             new SwordSlashSkill(true) // Finger of the Anubis
            //         }
            //     }
            // },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        private static readonly UnitScheme MissionaryHero = new()
        {
            TankRank = 0.2f,
            DamageDealerRank = 0.0f,
            SupportRank = 0.8f,

            Name = UnitName.Cheng,

            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new DopeHerbSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new DopeHerbSkill(),
            //             new PowerUpSkill(true)
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new DopeHerbSkill(), // No violence, please
            //             new PowerUpSkill(true), // Trust
            //             new HealSkill(true) // God Merciful Touch
            //         }
            //     }
            // },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public UnitSchemeCatalog()
        {
            PlayerUnits = new[]
            {
                SwordsmanHero,
                ArcherHero,
                HerbalistHero,

                MonkHero,
                SpearmanHero,
                MissionaryHero,

                ScorpionHero,
                PriestHero
            }.ToDictionary(scheme => scheme.Name, scheme => scheme);

            var slavicMonsters = CreateSlavicMonsters();
            var chineseMonsters = CreateChineseMonsters();
            var egyptianMonsters = CreateEgyptianMonsters();
            var greekMonsters = CreateGreekMonsters();

            AllMonsters = slavicMonsters.Concat(chineseMonsters).Concat(egyptianMonsters).Concat(greekMonsters)
                .ToArray();
        }

        private static IEnumerable<UnitScheme> CreateChineseMonsters()
        {
            var biomeType = BiomeType.Chinese;
            return new[]
            {
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.ChineseMonster,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.Monastery, GlobeNodeSid.GaintBamboo, GlobeNodeSid.EmperorTomb,
                        GlobeNodeSid.SecretTown, GlobeNodeSid.GreatWall, GlobeNodeSid.RiseFields,
                        GlobeNodeSid.DragonOolong, GlobeNodeSid.SkyTower
                    },
                    IsMonster = true,
                    
                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Taote,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.SkyTower },

                    BossLevel = 2,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill()),
                        new AddSkillUnitLevel(1, new ArrowRainSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateEgyptianMonsters()
        {
            var biomeType = BiomeType.Egyptian;
            return new[]
            {
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.EqyptianMonster,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.SacredPlace, GlobeNodeSid.Temple, GlobeNodeSid.Oasis, GlobeNodeSid.Obelisk,
                        GlobeNodeSid.ScreamValey
                    },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },

                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Sphynx,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.ScreamValey },

                    BossLevel = 3,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill()),
                        new AddSkillUnitLevel(1, new ArrowRainSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateGreekMonsters()
        {
            var biomeType = BiomeType.Greek;
            return new[]
            {
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.GreekMonster,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.ShipGraveyard, GlobeNodeSid.Vines, GlobeNodeSid.Garden, GlobeNodeSid.Palace,
                        GlobeNodeSid.Labirinth
                    },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Hydra,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Labirinth },

                    BossLevel = 4,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill()),
                        new AddSkillUnitLevel(1, new ArrowRainSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateSlavicMonsters()
        {
            var biomeType = BiomeType.Slavic;
            return new[]
            {
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Aspid,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Thicket, GlobeNodeSid.Swamp },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new SnakeBiteSkill()),
                        new AddPerkUnitLevel(3, new Evasion())
                    },

                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },

                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.GreyWolf,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Thicket, GlobeNodeSid.Battleground },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new WolfBiteSkill()),
                        new AddPerkUnitLevel(3, new CriticalHit())
                    },
                    
                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    LocationSids = new[]
                        { GlobeNodeSid.Battleground, GlobeNodeSid.Battleground, GlobeNodeSid.DeathPath },
                    IsUnique = true,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new BearBludgeonSkill()),
                        new AddPerkUnitLevel(3, new ImprovedHitPoints()),
                        new AddPerkUnitLevel(10, new ImprovedArmor()),
                    },

                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Swamp, GlobeNodeSid.DeathPath },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new WispEnergySkill())
                    },
                    
                    UnitGraphicsConfig = new WispMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.VolkolakWarrior,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.DeathPath, GlobeNodeSid.Mines },
                    IsUnique = true,
                    IsMonster = true,
                    IsBig = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new VolkolakEnergySkill()),
                        new AddPerkUnitLevel(10, new CriticalHit())
                    },
                    
                    UnitGraphicsConfig = new VolkolakWarriorGraphicsConfig(),

                    SchemeAutoTransition = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.5f,
                        NextScheme = new UnitScheme
                        {
                            TankRank = 0.0f,
                            DamageDealerRank = 0.5f,
                            SupportRank = 0.5f,

                            Name = UnitName.Volkolak,
                            Biome = biomeType,
                            IsUnique = true,
                            IsMonster = true,

                            Levels = new IUnitLevelScheme[]
                            {
                                new AddSkillUnitLevel(1, new VolkolakClawsSkill()),
                                new AddPerkUnitLevel(1, new ImprovedHitPoints()),
                                new AddPerkUnitLevel(10, new Evasion())
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Hermitage },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MassHealSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Stryga,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
                    IsMonster = true,
                    
                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Vampire,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp, GlobeNodeSid.Castle },
                    IsUnique = true,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new VampireBiteSkill()),
                        new AddPerkUnitLevel(5, new Evasion())
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 1.0f,
                    DamageDealerRank = 0.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.HornedFrog,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new DefenseStanceSkill())
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Basilisk,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
                    IsUnique = true,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new WideSlashSkill())
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.3f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.2f,

                    Name = UnitName.KosheyTheImmortal,
                    Biome = biomeType,

                    BossLevel = 1,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill()),
                        new AddSkillUnitLevel(1, new ArrowRainSkill()),
                    },
                    
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig(),
                    LocationSids = new[] { GlobeNodeSid.Castle },
                    MinRequiredBiomeLevel = 10,

                    SchemeAutoTransition = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.6f,
                        NextScheme = new UnitScheme
                        {
                            TankRank = 0.0f,
                            DamageDealerRank = 1.0f,
                            SupportRank = 0.0f,

                            Name = UnitName.KosheyTheImmortal2, // Dead-golem form
                            Biome = BiomeType.Slavic,
                            IsMonster = true,
                            BossLevel = 1,

                            Levels = new IUnitLevelScheme[]
                            {
                                new AddSkillUnitLevel(1, new WideSlashSkill()), // Bite
                                new AddSkillUnitLevel(1, new DefenseStanceSkill()), // Dead one hard to die
                                new AddSkillUnitLevel(1, new HealSkill()), // Eat a flash
                            },
                            
                            UnitGraphicsConfig = new KocheyDeadFormGraphicsConfig(),

                            SchemeAutoTransition = new UnitSchemeAutoTransition
                            {
                                HpShare = 0.3f,
                                NextScheme = new UnitScheme
                                {
                                    TankRank = 0.0f,
                                    DamageDealerRank = 1.0f,
                                    SupportRank = 0.0f,

                                    Name = UnitName.KosheyTheImmortal3, // Gaint spiritual face
                                    Biome = BiomeType.Slavic,
                                    IsMonster = true,
                                    BossLevel = 1,

                                    Levels = new IUnitLevelScheme[]
                                    {
                                        new AddSkillUnitLevel(1, new WispEnergySkill()), // Dark Wind
                                        new AddSkillUnitLevel(1, new DopeHerbSkill()), // Scary Eyes
                                        new AddSkillUnitLevel(1, new PowerUpSkill()), // 1000-years hate
                                    },
                                    
                                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                                }
                            }
                        }
                    }
                }
            };
        }

        public IDictionary<UnitName, UnitScheme> PlayerUnits { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}