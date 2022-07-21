using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    internal sealed class GreyWolfFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.GreyWolf,
                LocationSids = new[]
                    {
                        GlobeNodeSid.Thicket, GlobeNodeSid.Battleground, GlobeNodeSid.DestroyedVillage,
                        GlobeNodeSid.Swamp
                    },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                    {
                        new AddSkillUnitLevel(1, new WolfBiteSkill()),
                        new AddPerkUnitLevel(3, new CriticalHit())
                    },

                UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
            };
        }
    }
}
