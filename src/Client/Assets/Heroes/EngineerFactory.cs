using Client.Core;

using Rpg.Client.Assets.Equipments.Engineer;

namespace Client.Assets.Heroes;

internal class EngineerFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Engineer;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new FlameThrower(),
            new HeavyCooperHandmadeArmor(),
            new ScientificTableOfMaterials()
        };
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}