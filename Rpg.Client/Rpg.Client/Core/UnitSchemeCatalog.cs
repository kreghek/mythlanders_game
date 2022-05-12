using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets;
using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Assets.Skills.Hero.Swordsman;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Assets.Skills.Monster.Slavic;

namespace Rpg.Client.Core
{
    internal sealed class UnitSchemeCatalog : IUnitSchemeCatalog
    {
        public UnitSchemeCatalog(IBalanceTable balanceTable)
        {
            var heroes = new IHeroFactory[]
            {
                new SergeantFactory(),
                new AssaulterFactory(),
                new CryptoZoologistFactory(),

                new SwordsmanFactory(),
                new ArcherFactory(),
                new HerbalistFactory(),

                new MonkFactory(),
                new SpearmanFactory(),
                new SageFactory(),

                new ScorpionFactory(),
                new PriestFactory(),
                new LiberatorFactory(),

                new LegionnaireFactory(),
                new AmazonFactory(),
                new EngineerFactory()
            };

            Heroes = heroes.Select(x => x.Create(balanceTable)).ToDictionary(scheme => scheme.Name, scheme => scheme);

            var slavicMonsters = CreateSlavicMonsters(balanceTable);
            var chineseMonsters = CreateChineseMonsters(balanceTable);
            var egyptianMonsters = CreateEgyptianMonsters(balanceTable);
            var greekMonsters = CreateGreekMonsters(balanceTable);

            AllMonsters = slavicMonsters.Concat(chineseMonsters).Concat(egyptianMonsters).Concat(greekMonsters)
                .ToArray();
        }

        private static IEnumerable<UnitScheme> CreateChineseMonsters(IBalanceTable balanceTable)
        {
            var biomeType = BiomeType.Chinese;
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                        new AddSkillUnitLevel(1, new MonsterAttackSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Taote,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.SkyTower },

                    IsMonster = true,
                    MinRequiredBiomeLevel = 25,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddPerkUnitLevel(1, new BossMonster(2)),
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill()),
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateEgyptianMonsters(IBalanceTable balanceTable)
        {
            var biomeType = BiomeType.Egyptian;
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Mummy,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.SacredPlace, GlobeNodeSid.Temple, GlobeNodeSid.Oasis, GlobeNodeSid.Obelisk,
                        GlobeNodeSid.ScreamValey
                    },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },

                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Sphynx,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.ScreamValey },

                    IsMonster = true,

                    MinRequiredBiomeLevel = 35,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddPerkUnitLevel(1, new BossMonster(3)),
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateGreekMonsters(IBalanceTable balanceTable)
        {
            var biomeType = BiomeType.Greek;
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                        new AddSkillUnitLevel(1, new MonsterAttackSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Hydra,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Labirinth },

                    IsMonster = true,
                    MinRequiredBiomeLevel = 45,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddPerkUnitLevel(1, new BossMonster(4)),
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddSkillUnitLevel(1, new DopeHerbSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateSlavicMonsters(IBalanceTable balanceTable)
        {
            var biomeType = BiomeType.Slavic;
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,
                    Resolve = 9,

                    Name = UnitName.Marauder,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.Thicket, GlobeNodeSid.Swamp, GlobeNodeSid.Battleground, GlobeNodeSid.DeathPath,
                        GlobeNodeSid.Mines
                    },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new UnholyHitSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },

                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.25f,
                    DamageDealerRank = 0.75f,
                    SupportRank = 0.0f,
                    Resolve = 9,

                    Name = UnitName.BlackTrooper,
                    Biome = biomeType,
                    LocationSids = new[]
                    {
                        GlobeNodeSid.Thicket, GlobeNodeSid.Swamp, GlobeNodeSid.Battleground, GlobeNodeSid.DeathPath,
                        GlobeNodeSid.Mines
                    },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new BlackRifleShotSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },

                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,
                    Resolve = 13,

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

                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                        new AddPerkUnitLevel(1, new BigMonster())
                    },

                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.VolkolakWarrior,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.DeathPath, GlobeNodeSid.Mines },
                    IsUnique = true,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new VolkolakEnergySkill()),
                        new AddPerkUnitLevel(1, new BigMonster()),
                        new AddPerkUnitLevel(10, new CriticalHit())
                    },

                    UnitGraphicsConfig = new VolkolakWarriorGraphicsConfig(),

                    SchemeAutoTransition = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.5f,
                        NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                        {
                            TankRank = 0.5f,
                            DamageDealerRank = 0.5f,
                            SupportRank = 0.0f,

                            Name = UnitName.Volkolak,
                            Biome = biomeType,
                            IsUnique = true,
                            IsMonster = true,

                            Levels = new IUnitLevelScheme[]
                            {
                                new AddSkillUnitLevel(1, new VolkolakClawsSkill()),
                                new AddPerkUnitLevel(1, new ImprovedHitPoints()),
                                new AddPerkUnitLevel(5, new Evasion()),
                                new AddPerkUnitLevel(10, new CriticalHit())
                            },

                            UnitGraphicsConfig = new VolkolakGraphicsConfig()
                        }
                    }
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Korgorush,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.DestroyedVillage },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MassHealSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                        new AddSkillUnitLevel(1, new FrogDefenseStanceSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                },
                new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.3f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.2f,

                    Name = UnitName.KosheyTheImmortal,
                    Biome = biomeType,

                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new MonsterAttackSkill()),
                        new AddPerkUnitLevel(1, new BossMonster(1)),
                        new AddSkillUnitLevel(1, new DopeHerbSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig(),
                    LocationSids = new[] { GlobeNodeSid.Castle },
                    MinRequiredBiomeLevel = 10,

                    SchemeAutoTransition = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.6f,
                        NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                        {
                            TankRank = 0.0f,
                            DamageDealerRank = 1.0f,
                            SupportRank = 0.0f,

                            Name = UnitName.KosheyTheImmortal2, // Dead-golem form
                            Biome = BiomeType.Slavic,
                            IsMonster = true,

                            Levels = new IUnitLevelScheme[]
                            {
                                new AddPerkUnitLevel(1, new BossMonster(1)),
                                new AddSkillUnitLevel(1, new WideSlashSkill()), // Bite
                                new AddSkillUnitLevel(1, new DefenseStanceSkill()), // Dead one hard to die
                                new AddSkillUnitLevel(1, new HealSkill()) // Eat a flash
                            },

                            UnitGraphicsConfig = new KocheyDeadFormGraphicsConfig(),

                            SchemeAutoTransition = new UnitSchemeAutoTransition
                            {
                                HpShare = 0.3f,
                                NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                                {
                                    TankRank = 0.0f,
                                    DamageDealerRank = 1.0f,
                                    SupportRank = 0.0f,

                                    Name = UnitName.KosheyTheImmortal3, // Gaint spiritual face
                                    Biome = BiomeType.Slavic,
                                    IsMonster = true,

                                    Levels = new IUnitLevelScheme[]
                                    {
                                        new AddPerkUnitLevel(1, new BossMonster(1)),
                                        new AddSkillUnitLevel(1, new WispEnergySkill()), // Dark Wind
                                        new AddSkillUnitLevel(1, new DopeHerbSkill()), // Scary Eyes
                                        new AddSkillUnitLevel(1, new PowerUpSkill()) // 1000-years hate
                                    },

                                    UnitGraphicsConfig = new SingleSpriteMonsterGraphicsConfig()
                                }
                            }
                        }
                    }
                }
            };
        }

        public IDictionary<UnitName, UnitScheme> Heroes { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}