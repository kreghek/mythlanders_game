using Client.Core;

using Rpg.Client.Assets.Equipments.Spearman;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SpearmanFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Guardian;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EliteGuardsmanSpear(),
                new JuggernautHeavyPowerArmor(),
                new ChaoticNeuroInterface()
            };
        }

        protected override UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new GuardsmanGraphicsConfig();
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}