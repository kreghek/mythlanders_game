using Client.Assets.GraphicConfigs.Monsters.Black;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class AgressorFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.Agressor;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.25f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.0f,
            Resolve = 9,

            Name = UnitName.Agressor,
            LocationSids = new[]
            {
                LocationSids.Swamp, LocationSids.Battleground, LocationSids.DeathPath,
                LocationSids.Mines,

                LocationSids.Desert, LocationSids.SacredPlace,

                LocationSids.ShipGraveyard,

                LocationSids.Monastery
            },
            IsMonster = true
        };
    }

    public CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new AgressorGraphicsConfig(ClassName);
    }
}