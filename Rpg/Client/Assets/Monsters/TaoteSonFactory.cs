using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class TaoteSonFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Taote;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Taote,
                LocationSids = new[] { LocationSid.SkyTower },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<MonsterAttackSkill>(1),
                    new AddPerkUnitLevel<BossMonster>(1)
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