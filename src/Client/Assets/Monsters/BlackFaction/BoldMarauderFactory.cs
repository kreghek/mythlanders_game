using Client.Assets.GraphicConfigs.Monsters.Black;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class BoldMarauderFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.BoldMarauder;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.5f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.0f,
            Resolve = 9,

            Name = UnitName.BoldMarauder,
            LocationSids = new[]
            {
                LocationSids.Thicket, LocationSids.Swamp, LocationSids.Battleground, LocationSids.DeathPath,
                LocationSids.Mines,

                LocationSids.Desert, LocationSids.SacredPlace,

                LocationSids.ShipGraveyard,

                LocationSids.Monastery
            }
        };
    }

    public CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new MarauderGraphicsConfig(ClassName);
    }
}