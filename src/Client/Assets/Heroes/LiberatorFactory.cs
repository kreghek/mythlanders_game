using Client.Assets.Equipments.Liberator;
using Client.Core;

namespace Client.Assets.Heroes;

internal class LiberatorFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Liberator;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new VoiceModulator(),
            new HiddenExoskeleton(),
            new NewLawCodexOfFreedom()
        };
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}