using Client.Assets.GraphicConfigs.Monsters;

using JetBrains.Annotations;

using Rpg.Client.Assets.Monsters;
using Rpg.Client.Core;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class VampireFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Vampire;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            Name = UnitName.Vampire,
            LocationSids = new[]
            {
                LocationSids.Pit, LocationSids.DestroyedVillage, LocationSids.Castle
            },
            IsMonster = true,

            Levels = new IUnitLevelScheme[]
            {
            }
        };
    }
}