using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;
using Client.Core;

using Rpg.Client.Assets.Equipments.Amazon;
using Rpg.Client.Assets.Heroes;
using Rpg.Client.Core;

namespace Client.Assets.Heroes;

internal class AmazonFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Amazon;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new HunterRifle(),
            new TribeHunterScoutArmor(),
            new TheClawKnife()
        };
    }

    protected override UnitGraphicsConfigBase GetGraphicsConfig()
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