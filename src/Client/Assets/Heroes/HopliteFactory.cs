using Client.Core;

using Rpg.Client.Assets.Equipments.Legionnaire;
using Rpg.Client.Assets.GraphicConfigs.Heroes;

namespace Client.Assets.Heroes;

internal class HopliteFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Hoplite;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new EmberDori(),
            new EmpoweredMk2MediumPowerArmor(),
            new BrokenAresSculpture()
        };
    }

    protected override UnitGraphicsConfigBase GetGraphicsConfig()
    {
        return new HopliteGraphicsConfig(HeroName);
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}