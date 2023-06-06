using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class AspidFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Aspid;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics());
    }

    public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new AspidGraphicsConfig(ClassName);
    }
}