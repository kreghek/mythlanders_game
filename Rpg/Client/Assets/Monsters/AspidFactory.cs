using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class AspidFactory : MonsterFactoryBase
    {
        public override UnitName ClassName => UnitName.Aspid;

        public override UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = ClassName,
                LocationSids = new[]
                {
                    LocationSid.DestroyedVillage, LocationSid.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<SnakeBiteSkill>(1),
                    new AddPerkUnitLevel<Evasion>(3)
                },

                UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
            };
        }
    }
}