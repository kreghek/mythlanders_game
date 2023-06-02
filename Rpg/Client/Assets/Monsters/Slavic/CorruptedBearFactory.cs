using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Monsters;
using Rpg.Client.Core;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class CorruptedBearFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.CorruptedBear;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.5f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.0f,

            Name = UnitName.CorruptedBear,
            LocationSids = new[]
            {
                LocationSids.Battleground, LocationSids.DestroyedVillage, LocationSids.Swamp
            },
            IsMonster = true,

            Levels = new IUnitLevelScheme[]
            {
            }
        };
    }

    public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}