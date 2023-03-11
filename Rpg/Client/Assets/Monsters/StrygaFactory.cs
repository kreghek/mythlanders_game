using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class StrygaFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Stryga;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Stryga,
                LocationSids = new[]
                {
                    LocationSid.Pit, LocationSid.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<MonsterAttackSkill>(1)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new SingleSpriteGraphicsConfig();
        }
    }
}