using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

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