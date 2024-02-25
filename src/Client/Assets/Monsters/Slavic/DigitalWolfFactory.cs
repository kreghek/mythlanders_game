using Client.Assets.GraphicConfigs.Monsters.Slavic;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class DigitalWolfFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.DigitalWolf;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            Name = UnitName.DigitalWolf,
            LocationSids = new[]
            {
                LocationSids.Thicket, LocationSids.Battleground, LocationSids.DestroyedVillage,
                LocationSids.Swamp
            },
            IsMonster = true,

            Levels = new IUnitLevelScheme[]
            {
            }
        };
    }

    public CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new DigitalHortGraphicsConfig(ClassName);
    }
}