using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.Equipments.Sage;
using Client.Assets.GraphicConfigs;
using Client.Core;

namespace Client.Assets.Heroes;

internal class SageFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Sage;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new EmptinessInTheHand(),
            new DeceptivelyLightRobe(),
            new MagicAndMechanicalBox()
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