using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class WerewolfWarriorFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
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
                    new AddPerkUnitLevel<BigMonster>(1),
                    new AddSkillUnitLevel<VolkolakEnergySkill>(1),
                    new AddPerkUnitLevel<CriticalHit>(3)
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
                            new AddPerkUnitLevel<BigMonster>(1),
                            new AddSkillUnitLevel<VolkolakClawsSkill>(1),
                            new AddPerkUnitLevel<ImprovedHitPoints>(1),
                            new AddPerkUnitLevel<Evasion>(5),
                            new AddPerkUnitLevel<CriticalHit>(10)
                        },

                        UnitGraphicsConfig = new VolkolakGraphicsConfig()
                    }
                }
            };
        }
    }
}