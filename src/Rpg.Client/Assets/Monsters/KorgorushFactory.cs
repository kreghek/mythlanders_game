using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Hero.Herbalist;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class KorgorushFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Korgorush,
                LocationSids = new[]
                {
                    GlobeNodeSid.DestroyedVillage
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<MassHealSkill>(1),
                    new AddPerkUnitLevel<PowerUpAura>(1)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }
    }
}