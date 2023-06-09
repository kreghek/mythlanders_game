﻿using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class BlackTrooperFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.BlackTrooper;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.25f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.0f,
            Resolve = 9,

            Name = UnitName.BlackTrooper,
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
        return new BlackTrooperGraphicsConfig(ClassName);
    }
}