using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Swordsman;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class BasiliskFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 1.0f,
                DamageDealerRank = 0.0f,
                SupportRank = 0.0f,

                Name = UnitName.Basilisk,
                LocationSids = new[]
                {
                    GlobeNodeSid.Pit, GlobeNodeSid.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<WideSlashSkill>(1),
                    new AddPerkUnitLevel<BigMonster>(1)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }
    }
}