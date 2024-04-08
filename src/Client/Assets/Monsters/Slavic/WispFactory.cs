using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class WispFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Wisp;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}