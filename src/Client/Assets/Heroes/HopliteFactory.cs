using Client.Assets.Equipments.Legionnaire;
using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

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

    protected override CombatantGraphicsConfigBase GetGraphicsConfig()
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