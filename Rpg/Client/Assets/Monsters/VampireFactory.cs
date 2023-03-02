using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class VampireFactory : IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Vampire,
                LocationSids = new[]
                {
                    LocationSid.Pit, LocationSid.DestroyedVillage, LocationSid.Castle
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<VampireBiteSkill>(1),
                    new AddPerkUnitLevel<Evasion>(5)
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }
    }
}