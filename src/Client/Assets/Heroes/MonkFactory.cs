using Client.Assets.Equipments.Monk;
using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

using JetBrains.Annotations;

namespace Client.Assets.Heroes;

[UsedImplicitly]
internal class MonkFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Monk;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new RedemptionStaff(),
            new AsceticCape(),
            new SymbolOfGod()
        };
    }

    protected override UnitGraphicsConfigBase GetGraphicsConfig()
    {
        return new MaosinGraphicsConfig(HeroName);
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}