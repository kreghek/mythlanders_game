using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.Equipments.Assaulter;
using Client.Assets.GraphicConfigs;
using Client.Core;

namespace Client.Assets.Heroes;

internal class ShieldBearerFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.ShieldBearer;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new AssaultRifle(),
            new Mk4HeavyPowerArmor(),
            new LuckyPlayCard()
        };
    }

    protected override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes",
            HeroName.ToString(), "Thumbnail"));
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}