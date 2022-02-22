using Rpg.Client.Assets.Equipments;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills;
using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal class MonkBuilder : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Maosin;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new RedemptionStaff(),
                new AsceticRobe(),
                new SymbolOfGod()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new MaosinGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
                new AddSkillUnitLevel(1, new StaffHitSkill()),
                new AddSkillUnitLevel(2, new RestoreMantraSkill()),
                new AddPerkUnitLevel(2, new Evasion()),
                new AddSkillUnitLevel(3, new MasterStaffHitSkill(true)),
                new AddSkillUnitLevel(4, new GodNatureSkill(true))
            };
        }
    }
}