using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal static class UnitSchemeCatalog
    {
        public static UnitScheme SwordmanHero = new()
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
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseSkill(true),
                        new SvarogBlastFurnaceSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new BerimirGraphicsConfig()
        };

        public static UnitScheme MonkHero = new()
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
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StaffSkill(),
                        new DefenseSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new MaosinGraphicsConfig()
        };

        public static UnitScheme SpearmanHero = new()
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
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static UnitScheme ScorpionHero = new()
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
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SwordSlashSkill(),
                        new DefenseSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static UnitScheme HerbalistHero = new()
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

        public static UnitScheme ArcherHero = new()
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
                        new DefenseSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static UnitScheme PriestHero = new()
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

        public static UnitScheme MissionaryHero = new()
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
                        new HealSkill(true) // God Mercifull Touch
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        public static IDictionary<UnitName, UnitScheme> PlayerUnits = new[]
        {
            SwordmanHero,
            ArcherHero,
            HerbalistHero,

            MonkHero,
            SpearmanHero,
            MissionaryHero,

            ScorpionHero,
            PriestHero
        }.ToDictionary(scheme => scheme.Name, scheme => scheme);

        static UnitSchemeCatalog()
        {
            var slavicMonsters = CreateSlavicMonsters();
            var chineseMonsters = CreateChineseMonsters();
            var egyptianMonsters = CreateEgyptianMonsters();
            var greekMonsters = CreateGreekMonsters();

            AllUnits = slavicMonsters.Concat(chineseMonsters).Concat(egyptianMonsters).Concat(greekMonsters);
        }

        public static IEnumerable<UnitScheme> AllUnits { get; }

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
                                new MonsterAttackSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 0.4f,
                    SupportRank = 0.6f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    NodeIndexes = new[] { 1, 2, 4 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(), // Bite
                                new DefenseSkill(),
                                new WideSlashSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new BowShotSkill(),
                                new ArrowRainSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new HealSkill(),
                                new WideSlashSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new HealSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new MassHealSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.HornedFrog,
                    Biome = biomeType,
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DefenseSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new WideSlashSkill()
                            }
                        }
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

                    BossLevel = 2,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new ArrowRainSkill()
                            }
                        }
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
                                new MonsterAttackSkill(), // Bite
                                new PowerUpSkill(), // Wolf howl
                                new HealSkill() // lick wounds
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    NodeIndexes = new[] { 1, 2, 4 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(), // Bite
                                new DefenseSkill(),
                                new WideSlashSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new BowShotSkill(),
                                new ArrowRainSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new HealSkill(),
                                new WideSlashSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new HealSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new MassHealSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.HornedFrog,
                    Biome = biomeType,
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DefenseSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new WideSlashSkill()
                            }
                        }
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

                    BossLevel = 3,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new ArrowRainSkill()
                            }
                        }
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
                                new MonsterAttackSkill(), // Bite
                                new PowerUpSkill(), // Wolf howl
                                new HealSkill() // lick wounds
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    NodeIndexes = new[] { 1, 2, 4 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(), // Bite
                                new DefenseSkill(),
                                new WideSlashSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new BowShotSkill(),
                                new ArrowRainSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new PowerUpSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new HealSkill(),
                                new WideSlashSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new WideSlashSkill(),
                                new HealSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new MassHealSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.HornedFrog,
                    Biome = biomeType,
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DefenseSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new WideSlashSkill()
                            }
                        }
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

                    BossLevel = 4,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new ArrowRainSkill()
                            }
                        }
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
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    NodeIndexes = new[] { 1, 2, 4 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new WolfBiteSkill() // Bite
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new ArrowRainSkill()
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
                    NodeIndexes = new[] { 2, 3, 5 },
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
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new HealSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new VampiricBiteSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.HornedFrog,
                    Biome = biomeType,
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new DefenseSkill()
                            }
                        }
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
                    NodeIndexes = new[] { 7, 8, 9 },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new WideSlashSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.KosheyTheImmortal,
                    Biome = biomeType,

                    BossLevel = 1,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill(),
                                new DopeHerbSkill(),
                                new ArrowRainSkill()
                            }
                        }
                    },
                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig(),

                    SchemeAutoTransition = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.6f,
                        NextScheme = new UnitScheme
                        {
                            TankRank = 0.0f,
                            DamageDealerRank = 1.0f,
                            SupportRank = 0.0f,

                            Name = UnitName.KosheyTheImmortal2,
                            Biome = BiomeType.Slavic,
                            NodeIndexes = new[] { 0, 1, 2 },
                            IsMonster = true,

                            SkillSets = new List<SkillSet>
                            {
                                new SkillSet
                                {
                                    Skills = new List<SkillBase>
                                    {
                                        new MonsterAttackSkill(), // Bite
                                        new DefenseSkill(), // Dead one hard to die
                                        new HealSkill() // Eat a flash
                                    }
                                }
                            },
                            UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig(),

                            SchemeAutoTransition = new UnitSchemeAutoTransition
                            {
                                HpShare = 0.3f,
                                NextScheme = new UnitScheme
                                {
                                    TankRank = 0.0f,
                                    DamageDealerRank = 1.0f,
                                    SupportRank = 0.0f,

                                    Name = UnitName.KosheyTheImmortal3,
                                    Biome = BiomeType.Slavic,
                                    NodeIndexes = new[] { 0, 1, 2 },
                                    IsMonster = true,

                                    SkillSets = new List<SkillSet>
                                    {
                                        new SkillSet
                                        {
                                            Skills = new List<SkillBase>
                                            {
                                                new ArrowRainSkill(), // Dark Wind
                                                new DopeHerbSkill(), // Scary Eyes
                                                new PowerUpSkill() // 1000-years hate
                                            }
                                        }
                                    },
                                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}