using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal class UnitSchemeCatalog : IUnitSchemeCatalog
    {
        private static readonly UnitScheme SwordsmanHero = new()
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

        private static readonly UnitScheme MonkHero = new()
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

        private static readonly UnitScheme SpearmanHero = new()
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
                        new HealingSalveSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new HealingSalveSkill(),
                        new DopeHerbSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new HealingSalveSkill(),
                        new DopeHerbSkill(true),
                        new MassHealSkill(true)
                    }
                }
            },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        private static readonly UnitScheme ArcherHero = new()
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
            UnitGraphicsConfig = new HawkGraphicsConfig()
        };

        private static readonly UnitScheme PriestHero = new()
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

        private static readonly UnitScheme MissionaryHero = new()
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

                    Name = UnitName.GreyWolf,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Monastery, GlobeNodeSid.GaintBamboo, GlobeNodeSid.EmperorTomb, GlobeNodeSid.SecretTown, GlobeNodeSid.GreatWall, GlobeNodeSid.RiseFields, GlobeNodeSid.DragonOolong, GlobeNodeSid.SkyTower },
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

                    Name = UnitName.Taote,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.SkyTower },

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
                    LocationSids = new[] { GlobeNodeSid.SacredPlace, GlobeNodeSid.Temple, GlobeNodeSid.Oasis, GlobeNodeSid.Obelisk, GlobeNodeSid.ScreamValey },
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

                    Name = UnitName.Sphynx,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.ScreamValey },

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
                    LocationSids = new[] { GlobeNodeSid.ShipGraveyard, GlobeNodeSid.Vines, GlobeNodeSid.Garden, GlobeNodeSid.Palace, GlobeNodeSid.Labirinth },
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

                    Name = UnitName.Hydra,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Labirinth },

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
                    LocationSids = new[] { GlobeNodeSid.Thicket, GlobeNodeSid.Swamp },
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
                    LocationSids = new[] { GlobeNodeSid.Thicket, GlobeNodeSid.Battleground },
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
                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig(),
                    Perks = new IPerk[]
                    {
                        new CriticalHit()
                    }
                },
                new UnitScheme
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Battleground, GlobeNodeSid.Battleground, GlobeNodeSid.DeathPath },
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
                    LocationSids = new[] { GlobeNodeSid.Swamp, GlobeNodeSid.DeathPath },
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
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.VolkolakWarrior,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.DeathPath, GlobeNodeSid.Mines },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new VolkolakEnergySkill()
                            }
                        }
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
                            UnitGraphicsConfig = new VolkolakGraphicsConfig(),
                            Perks = new[]
                            {
                                new ImprovedHitPoints()
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
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
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
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
                    IsUnique = true,
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new VampireBiteSkill()
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
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
                    IsMonster = true,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new DefenseStanceSkill()
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
                    LocationSids = new[] { GlobeNodeSid.Pit, GlobeNodeSid.Swamp },
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

                            Name = UnitName.KosheyTheImmortal2,
                            Biome = BiomeType.Slavic,
                            IsMonster = true,

                            SkillSets = new List<SkillSet>
                            {
                                new SkillSet
                                {
                                    Skills = new List<SkillBase>
                                    {
                                        new MonsterAttackSkill(), // Bite
                                        new DefenseStanceSkill(), // Dead one hard to die
                                        new HealSkill() // Eat a flash
                                    }
                                }
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

                                    Name = UnitName.KosheyTheImmortal3,
                                    Biome = BiomeType.Slavic,
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

        public IDictionary<UnitName, UnitScheme> PlayerUnits { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}