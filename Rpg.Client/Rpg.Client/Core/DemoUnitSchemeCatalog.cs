﻿using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.GraphicConfigs;
using Rpg.Client.Core.Perks;

namespace Rpg.Client.Core
{
    internal sealed class DemoUnitSchemeCatalog : IUnitSchemeCatalog
    {
        private static readonly UnitScheme SwordsmanHero = new()
        {
            TankRank = 0.4f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.1f,

            Name = UnitName.Berimir,
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
            //             new DefenseStanceSkill(true),
            //             new SvarogBlastFurnaceSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new BerimirGraphicsConfig()
        };

        private static readonly UnitScheme HerbalistHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.0f,
            SupportRank = 1.0f,

            Name = UnitName.Rada,

            // SkillSets = new List<SkillSet>
            // {
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new HealingSalveSkill()
            //         }
            //     },
            //     new SkillSet
            //     {
            //         Skills = new List<SkillBase>
            //         {
            //             new HealingSalveSkill(),
            //             new DopeHerbSkill(true),
            //             new MassHealSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new GenericCharacterGraphicsConfig()
        };

        private static readonly UnitScheme ArcherHero = new()
        {
            TankRank = 0.0f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.25f,

            Name = UnitName.Hawk,

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
            //             new ArrowRainSkill(true),
            //             new DefenseStanceSkill(true)
            //         }
            //     }
            // },
            UnitGraphicsConfig = new HawkGraphicsConfig()
        };

        public DemoUnitSchemeCatalog()
        {
            PlayerUnits = new[]
            {
                SwordsmanHero,
                ArcherHero,
                HerbalistHero
            }.ToDictionary(scheme => scheme.Name, scheme => scheme);

            var slavicMonsters = CreateSlavicMonsters();

            AllMonsters = slavicMonsters.ToArray();
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

                    // SkillSets = new List<SkillSet>
                    // {
                    //     new SkillSet
                    //     {
                    //         Skills = new List<SkillBase>
                    //         {
                    //             new SnakeBiteSkill()
                    //         }
                    //     }
                    // },

                    // Perks = new[]
                    // {
                    //     new Evasion()
                    // },
                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },

                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.GreyWolf,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Thicket, GlobeNodeSid.Swamp },
                    IsMonster = true,

                    // SkillSets = new List<SkillSet>
                    // {
                    //     new SkillSet
                    //     {
                    //         Skills = new List<SkillBase>
                    //         {
                    //             new WolfBiteSkill()
                    //         }
                    //     }
                    // },
                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Battleground },
                    IsUnique = true,
                    IsMonster = true,

                    // SkillSets = new List<SkillSet>
                    // {
                    //     new SkillSet
                    //     {
                    //         Skills = new List<SkillBase>
                    //         {
                    //             new BearBludgeonSkill()
                    //         }
                    //     }
                    // },

                    // Perks = new IPerk[]
                    // {
                    //     new ImprovedHitPoints(),
                    //     new ImprovedArmor()
                    // },

                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Wisp,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Swamp, GlobeNodeSid.Pit, GlobeNodeSid.DeathPath },
                    IsMonster = true,

                    // SkillSets = new List<SkillSet>
                    // {
                    //     new SkillSet
                    //     {
                    //         Skills = new List<SkillBase>
                    //         {
                    //             new WispEnergySkill()
                    //         }
                    //     }
                    // },
                    UnitGraphicsConfig = new WispMonsterGraphicsConfig()
                },
                new UnitScheme
                {
                    TankRank = 0.3f,
                    DamageDealerRank = 0.7f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    Biome = biomeType,
                    LocationSids = new[] { GlobeNodeSid.Battleground },
                    IsUnique = true,
                    IsMonster = true,
                    //BossLevel = 1,
                    MinRequiredBiomeLevel = 5,

                    // SkillSets = new List<SkillSet>
                    // {
                    //     new SkillSet
                    //     {
                    //         Skills = new List<SkillBase>
                    //         {
                    //             new VolkolakClawsSkill()
                    //         }
                    //     }
                    // },
                    UnitGraphicsConfig = new VolkolakGraphicsConfig()
                }
            };
        }

        public IDictionary<UnitName, UnitScheme> PlayerUnits { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}