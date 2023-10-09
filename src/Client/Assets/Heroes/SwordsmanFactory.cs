using Client.Assets.Equipments.Swordsman;
using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

namespace Client.Assets.Heroes;

internal class SwordsmanFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Swordsman;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new CombatSword(),
            new Mk2MediumPowerArmor(),
            new WoodenHandSculpture()
        };
    }

    protected override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new SwordsmanGraphicsConfig(HeroName);
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}