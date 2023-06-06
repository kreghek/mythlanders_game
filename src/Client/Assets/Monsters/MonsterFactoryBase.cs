using System.IO;

using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

namespace Client.Assets.Monsters;

internal abstract class MonsterFactoryBase : IMonsterFactory
{
    public abstract CharacterCultureSid Culture { get; }
    public abstract UnitName ClassName { get; }

    public abstract UnitScheme Create(IBalanceTable balanceTable);

    public virtual UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Monsters",
            Culture.ToString(), ClassName.ToString(), "Thumbnail"));
    }
}