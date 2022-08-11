using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class DigitalWolfFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.DigitalWolf,
                LocationSids = new[]
                {
                    GlobeNodeSid.Thicket, GlobeNodeSid.Battleground, GlobeNodeSid.DestroyedVillage,
                    GlobeNodeSid.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<DigitalBiteSkill>(1),
                    new AddPerkUnitLevel<CriticalHit>(3)
                },

                UnitGraphicsConfig = new DigitalWolfGraphicsConfig()
            };
        }
    }
}