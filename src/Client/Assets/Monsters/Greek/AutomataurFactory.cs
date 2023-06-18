using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

namespace Client.Assets.Monsters.Greek;
internal class AutomataurFactory: MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Automataur;

    public override CharacterCultureSid Culture => CharacterCultureSid.Greek;

    public override UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics());
    }

    public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Monsters", Culture.ToString(),
            ClassName.ToString(), "Thumbnail"));
    }
}
