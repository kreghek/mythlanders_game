using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.Equipments.Zoologist;
using Client.Assets.GraphicConfigs;
using Client.Core;

namespace Client.Assets.Heroes;

internal class CryptoZoologistFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Zoologist;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new BioExtractor(),
            new ScientistRobe(),
            new DetectiveOculars()
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