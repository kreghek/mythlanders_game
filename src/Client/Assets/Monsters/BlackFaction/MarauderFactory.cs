﻿using Client.Assets;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class MarauderFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.Marauder;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.5f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.0f,
            Resolve = 9,

            Name = UnitName.Marauder,
            LocationSids = new[]
            {
                LocationSids.Thicket, LocationSids.Swamp, LocationSids.Battleground, LocationSids.DeathPath,
                LocationSids.Mines,

                LocationSids.Desert, LocationSids.SacredPlace,

                LocationSids.ShipGraveyard,

                LocationSids.Monastery
            },
            IsMonster = true
        };
    }

    public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new MarauderGraphicsConfig(ClassName);
    }
}