using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class BlackTrooperFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.25f,
                DamageDealerRank = 0.75f,
                SupportRank = 0.0f,
                Resolve = 9,

                Name = UnitName.BlackTrooper,
                LocationSids = new[]
                {
                    GlobeNodeSid.Thicket, GlobeNodeSid.Swamp, GlobeNodeSid.Battleground, GlobeNodeSid.DeathPath,
                    GlobeNodeSid.Mines,

                    GlobeNodeSid.Desert, GlobeNodeSid.SacredPlace,

                    GlobeNodeSid.ShipGraveyard,

                    GlobeNodeSid.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<BlackRifleShotSkill>(1)
                },

                UnitGraphicsConfig = new BlackTrooperGraphicsConfig()
            };
        }
    }
}