using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class AmbushDroneFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.AmbushDrone;
    public override CharacterCultureSid Culture => CharacterCultureSid.Black;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.25f,
            DamageDealerRank = 0.75f,
            SupportRank = 0.0f,
            Resolve = 9,

            Name = UnitName.AmbushDrone,
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

    public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}