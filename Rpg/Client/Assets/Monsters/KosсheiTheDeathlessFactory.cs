using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Herbalist;
using Rpg.Client.Assets.Skills.Hero.Swordsman;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class KosсheiTheDeathlessFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.3f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.2f,

                Name = UnitName.KosheyTheImmortal,

                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<MonsterAttackSkill>(1),
                    new AddPerkUnitLevel<BossMonster>(1),
                    new AddSkillUnitLevel<DopeHerbSkill>(1)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig(),
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
                        IsMonster = true,

                        Levels = new IUnitLevelScheme[]
                        {
                            new AddPerkUnitLevel<BossMonster>(1),
                            new AddSkillUnitLevel<WideSlashSkill>(1), // Bite
                            new AddSkillUnitLevel<DefenseStanceSkill>(1), // Dead one hard to die
                            new AddSkillUnitLevel<HealSkill>(1) // Eat a flash
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
                                IsMonster = true,

                                Levels = new IUnitLevelScheme[]
                                {
                                    new AddPerkUnitLevel<BossMonster>(1),
                                    new AddSkillUnitLevel<WispEnergySkill>(1), // Dark Wind
                                    new AddSkillUnitLevel<DopeHerbSkill>(1), // Scary Eyes
                                    new AddSkillUnitLevel<PowerUpSkill>(1) // 1000-years hate
                                },

                                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
                            }
                        }
                    }
                }
            };
        }
    }
}