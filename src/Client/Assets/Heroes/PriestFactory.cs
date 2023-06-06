using Client.Core;

using Rpg.Client.Assets.Equipments.Priest;

namespace Client.Assets.Heroes;

internal class PriestFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Priest;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new EgyptianBookOfDeath(),
            new NanoMetalLongCloths(),
            new ScarabeusKingLeg()
        };
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}