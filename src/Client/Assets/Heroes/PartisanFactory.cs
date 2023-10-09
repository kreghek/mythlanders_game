using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.Equipments.Sergeant;
using Client.Assets.GraphicConfigs;
using Client.Core;

namespace Client.Assets.Heroes;

internal class PartisanFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Partisan;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new CompactSaber(),
            new RedMediumPowerArmor(),
            new MultifunctionalClocks()
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