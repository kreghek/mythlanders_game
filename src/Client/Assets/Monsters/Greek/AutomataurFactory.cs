using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Greek;

[UsedImplicitly]
internal class AutomataurFactory: MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Automataur;

    public override CharacterCultureSid Culture => CharacterCultureSid.Greek;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics());
    }
}
