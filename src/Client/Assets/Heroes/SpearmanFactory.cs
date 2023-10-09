using Client.Assets.Equipments.Spearman;
using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;

namespace Client.Assets.Heroes;

internal class SpearmanFactory : HeroFactoryBase
{
    public override UnitName HeroName => UnitName.Guardian;

    protected override IEquipmentScheme[] GetEquipment()
    {
        return new IEquipmentScheme[]
        {
            new EliteGuardsmanSpear(),
            new JuggernautHeavyPowerArmor(),
            new ChaoticNeuroInterface()
        };
    }

    protected override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new GuardsmanGraphicsConfig(HeroName);
    }

    protected override IUnitLevelScheme[] GetLevels()
    {
        return new IUnitLevelScheme[]
        {
        };
    }
}