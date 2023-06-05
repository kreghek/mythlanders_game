using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Monsters;
using Rpg.Client.Core;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class WispFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Wisp;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            Name = UnitName.Wisp,
            LocationSids = new[]
            {
                LocationSids.DestroyedVillage, LocationSids.Swamp
            },
            IsMonster = true
        };
    }

    public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}