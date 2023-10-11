using Client.Assets.Equipments.Archer;
using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

namespace Client.Assets.Heroes;

internal class RobberFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Robber;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new ArcherPulsarBow(),
            new Mk3ScoutPowerArmor(),
            new SilverWindNecklace()
        };
    }

    protected override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new RobberGraphicsConfig(HeroName);
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}