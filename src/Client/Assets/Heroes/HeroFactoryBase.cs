using Client.Assets.GraphicConfigs.Heroes;
using Client.Core;
using Client.GameScreens;

namespace Client.Assets.Heroes;

internal abstract class HeroFactoryBase : IHeroFactory
{
    protected abstract IEquipmentScheme[] GetEquipment();

    protected virtual CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new GenericHeroGraphicsConfig(HeroName);
    }

    protected abstract IUnitLevelScheme[] GetLevels();
    public abstract UnitName HeroName { get; }
    public virtual bool IsReleaseReady { get; } = true;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        var record = balanceTable.GetRecord(HeroName.ToString());

        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = record.TankRank,
            DamageDealerRank = record.DamageDealerRank,
            SupportRank = record.SupportRank,

            Name = HeroName,

            Levels = GetLevels(),

            Equipments = GetEquipment(),

            UnitGraphicsConfig = GetGraphicsConfig()
        };
    }

    public CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return GetGraphicsConfig();
    }
}