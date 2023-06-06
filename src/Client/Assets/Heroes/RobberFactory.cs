using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

using Rpg.Client.Assets.Equipments.Archer;

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

    protected override UnitGraphicsConfigBase GetGraphicsConfig()
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