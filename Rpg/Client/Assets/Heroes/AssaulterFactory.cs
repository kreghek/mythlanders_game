using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;
using System.IO;

using Client.Core;

using Rpg.Client.Assets.Equipments.Assaulter;
using Rpg.Client.Core;
using Rpg.Client.Assets.Heroes;

namespace Client.Assets.Heroes;

internal class AssaulterFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Assaulter;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new AssaultRifle(),
            new Mk4HeavyPowerArmor(),
            new LuckyPlayCard()
        };
    }

    protected override UnitGraphicsConfigBase GetGraphicsConfig()
    {
        return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes", HeroName.ToString(), "Thumbnail"));
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}