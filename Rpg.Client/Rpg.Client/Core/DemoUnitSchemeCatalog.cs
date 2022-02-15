using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Core
{
    internal sealed class DemoUnitSchemeCatalog : IUnitSchemeCatalog
    {
        public DemoUnitSchemeCatalog()
        {
            var heroes = new IHeroBuilder[]
            {
                new SwordsmanBuilder(),
                new ArcherFactory(),
                new HerbalistFactory()
            };

            var balanceTable = new BalanceTable();

            Heroes = heroes.Select(x => x.Create(balanceTable)).ToDictionary(scheme => scheme.Name, scheme => scheme);

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
                    LocationSids = new[] { GlobeNodeSid.DestroyedVillage },
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
                    LocationSids = new[] { GlobeNodeSid.Thicket },
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
                        { GlobeNodeSid.Battleground },
                    IsUnique = true,
                    IsMonster = true,

                    Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new BearBludgeonSkill()),
                        new AddPerkUnitLevel(1, new BigMonster())
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
                    LocationSids = new[] { GlobeNodeSid.DestroyedVillage },
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
                    LocationSids = new[] { GlobeNodeSid.Swamp },
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
                        NextScheme = new UnitScheme
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
                }
            };
        }

        public IDictionary<UnitName, UnitScheme> Heroes { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}