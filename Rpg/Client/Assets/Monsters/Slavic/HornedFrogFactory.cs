using Client.Assets.GraphicConfigs.Monsters;

using JetBrains.Annotations;

using Rpg.Client.Assets.Monsters;
using Rpg.Client.Core;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class HornedFrogFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.HornedFrog;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 1.0f,
            DamageDealerRank = 0.0f,
            SupportRank = 0.0f,

            Name = UnitName.HornedFrog,
            LocationSids = new[]
            {
                LocationSids.Pit, LocationSids.Swamp
            },
            IsMonster = true
        };
    }
}