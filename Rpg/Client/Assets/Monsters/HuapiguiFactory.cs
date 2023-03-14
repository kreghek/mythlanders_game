using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class HuapiguiFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Huapigui,
                LocationSids = new[]
                {
                    GlobeNodeSid.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<SnakeBiteSkill>(1),
                    new AddPerkUnitLevel<CriticalHit>(3)
                },

                UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
            };
        }
    }
}