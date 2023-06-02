using Rpg.Client.Assets.Equipments.Medjay;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Core;

namespace Client.Assets.Heroes;

internal class MedjayFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Medjay;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new UltraLightSaber(),
            new FireResistBlackArmor(),
            new GreenTattoo()
        };
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}