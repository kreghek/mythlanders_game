using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Assets.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DemoUnitSchemeCatalog : IUnitSchemeCatalog
    {
        public DemoUnitSchemeCatalog(IBalanceTable balanceTable)
        {
            var heroes = new IHeroFactory[]
            {
                new CommissarFactory(),
                new AssaulterFactory(),

                new SwordsmanFactory(),
                new ArcherFactory(),
                new HerbalistFactory(),

                new MonkFactory(),
                new SpearmanFactory(),

                new PriestFactory(),

                new AmazonFactory()
            };

            Heroes = heroes.Select(x => x.Create(balanceTable)).ToDictionary(scheme => scheme.Name, scheme => scheme);

            var monsterFactories = LoadFactories();
            var loadedMonsters = monsterFactories.Select(x => x.Create(balanceTable));

            var slavicMonsters = CreateSlavicMonsters(balanceTable);
            var chineseMonsters = CreateChineseMonsters(balanceTable);
            var egyptianMonsters = CreateEgyptianMonsters(balanceTable);

            AllMonsters = loadedMonsters.Concat(slavicMonsters).Concat(chineseMonsters).Concat(egyptianMonsters).ToArray();
        }

        private static IReadOnlyCollection<IMonsterFactory> LoadFactories()
        {
            var assembly = typeof(IMonsterFactory).Assembly;
            var factoryTypes = assembly.GetTypes().Where(x => typeof(IMonsterFactory).IsAssignableFrom(x) && x != typeof(IMonsterFactory));
            var factories = factoryTypes.Select(x => Activator.CreateInstance(x));
            return factories.OfType<IMonsterFactory>().ToArray();
        }

        private static IEnumerable<UnitScheme> CreateSlavicMonsters(IBalanceTable balanceTable)
        {
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Aspid,
                    LocationSids = new[] { GlobeNodeSid.DestroyedVillage, GlobeNodeSid.Swamp },
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
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.Bear,
                    LocationSids = new[]
                        { GlobeNodeSid.Battleground, GlobeNodeSid.DestroyedVillage, GlobeNodeSid.Swamp },
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
                    LocationSids = new[] { GlobeNodeSid.DestroyedVillage, GlobeNodeSid.Swamp },
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
                    LocationSids = new[] { GlobeNodeSid.Swamp },
                    IsUnique = true,
                    IsMonster = true,
                    MinRequiredBiomeLevel = 5,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddPerkUnitLevel(1, new BossMonster(1)),
                        new AddSkillUnitLevel(1, new VolkolakEnergySkill()),
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
                            IsUnique = true,
                            IsMonster = true,

                            Levels = new IUnitLevelScheme[]
                            {
                                new AddPerkUnitLevel(1, new BossMonster(1)),
                                new AddSkillUnitLevel(1, new VolkolakClawsSkill()),
                                new AddPerkUnitLevel(1, new ImprovedHitPoints()),
                                new AddPerkUnitLevel(5, new Evasion()),
                                new AddPerkUnitLevel(10, new CriticalHit())
                            },

                            UnitGraphicsConfig = new VolkolakGraphicsConfig()
                        }
                    }
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateChineseMonsters(IBalanceTable balanceTable)
        {
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Huapigui,
                    LocationSids = new[] { GlobeNodeSid.Monastery },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new SnakeBiteSkill())
                    },

                    UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
                }
            };
        }

        private static IEnumerable<UnitScheme> CreateEgyptianMonsters(IBalanceTable balanceTable)
        {
            return new[]
            {
                new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.0f,
                    DamageDealerRank = 1.0f,
                    SupportRank = 0.0f,

                    Name = UnitName.Mummy,
                    LocationSids = new[] {

                        GlobeNodeSid.Desert, GlobeNodeSid.SacredPlace },
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new VampireBiteSkill())
                    },

                    UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
                }
            };
        }

        public IDictionary<UnitName, UnitScheme> Heroes { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}