using JetBrains.Annotations;

using Rpg.Client.Assets.Equipments.Monk;
using Rpg.Client.Assets.GraphicConfigs.Heroes;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Core;

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
        return new MaosinGraphicsConfig();
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}