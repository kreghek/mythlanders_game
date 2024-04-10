using Client.Assets.GraphicConfigs.Monsters;
using Client.Assets.GraphicConfigs.Monsters.Slavic;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class DigitalWolfFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.DigitalWolf;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new DigitalWolfGraphicsConfig(ClassName);
    }
}