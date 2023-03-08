using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class MummyWarriorFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.MummyWarrior,
                LocationSids = new[]
                {
                    LocationSid.Desert
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<VampireBiteSkill>(1)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }
    }
}