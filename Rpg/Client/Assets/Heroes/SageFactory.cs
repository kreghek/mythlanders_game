using Rpg.Client.Assets.Equipments.Sage;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal class SageFactory : HeroFactoryBase
    {
        public override UnitName HeroName => UnitName.Sage;

        protected override IEquipmentScheme[] GetEquipment()
        {
            return new IEquipmentScheme[]
            {
                new EmptinessInTheHand(),
                new DeceptivelyLightRobe(),
                new MagicAndMechanicalBox()
            };
        }

        protected override IUnitLevelScheme[] GetLevels()
        {
            return new IUnitLevelScheme[]
            {
            };
        }
    }
}