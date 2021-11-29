using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class DemoUnitSchemeCatalog : IUnitSchemeCatalog
    {
        public static readonly UnitScheme SwordsmanHero = new()
        {
            TankRank = 0.4f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.1f,

            Name = UnitName.Berimir,
            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true),
                        new SvarogBlastFurnaceSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new BerimirGraphicsConfig()
        };

        public static readonly UnitScheme MonkHero = new()
        {
            TankRank = 0.2f,
            DamageDealerRank = 0.6f,
            SupportRank = 0.2f,

            Name = UnitName.Maosin,
            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StaffSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StaffSkill(),
                        new DefenseStanceSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StaffSkill(),
                        new DefenseStanceSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new MaosinGraphicsConfig()
        };

        public static readonly UnitScheme SpearmanHero = new()
        {
            TankRank = 0.8f,
            DamageDealerRank = 0.1f,
            SupportRank = 0.1f,

            Name = UnitName.Ping,
            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme ScorpionHero = new()
        {
            TankRank = 0.1f,
            DamageDealerRank = 0.8f,
            SupportRank = 0.1f,

            Name = UnitName.Amun,
            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseStanceSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme HerbalistHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.0f,
            SupportRank = 1.0f,

            Name = UnitName.Rada,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill(),
                        new DopeHerbSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill(),
                        new DopeHerbSkill(true),
                        new MassHealSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme ArcherHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.25f,

            Name = UnitName.Hawk,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill(),
                        new ArrowRainSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill(),
                        new ArrowRainSkill(true),
                        new DefenseStanceSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme PriestHero = new()
        {
            TankRank = 0.1f,
            DamageDealerRank = 0.9f,
            SupportRank = 0.0f,

            Name = UnitName.Kakhotep,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill(),
                        new MassStunSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new BowShotSkill(),
                        new MassStunSkill(true),
                        new SwordSlashSkill(true) // Finger of the Anubis
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static readonly UnitScheme MissionaryHero = new()
        {
            TankRank = 0.2f,
            DamageDealerRank = 0.0f,
            SupportRank = 0.8f,

            Name = UnitName.Cheng,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill(),
                        new PowerUpSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill(), // No violence, please
                        new PowerUpSkill(true), // Trust
                        new HealSkill(true) // God Merciful Touch
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public IDictionary<UnitName, UnitScheme> PlayerUnits { get; }

        public DemoUnitSchemeCatalog()
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

            AllMonsters = slavicMonsters.ToArray();
        }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }

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
                    NodeIndexes = new[] { 0, 1, 2 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new SnakeBiteSkill()
                            }
                        }
                    },

                    Perks = new[]
                    {
                        new Evasion()
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
                    NodeIndexes = new[] { 0, 1, 2 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new WolfBiteSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 1, 2 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new BearBludgeonSkill()
                            }
                        }
                    },

                    Perks = new IPerk[]
                    {
                        new ImprovedHitPoints(),
                        new ImprovedArmor()
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
                    NodeIndexes = new[] { 1, 2 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new WispEnergySkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new WispMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    Biome = biomeType,
                    NodeIndexes = new[] { 3 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new VolkolakClawsSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                }
            };
        }
    }
}