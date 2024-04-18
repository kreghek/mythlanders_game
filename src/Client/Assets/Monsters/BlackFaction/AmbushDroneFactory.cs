using Client.Assets.GraphicConfigs.Monsters;
using Client.Assets.GraphicConfigs.Monsters.Black;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.BlackFaction;

[UsedImplicitly]
internal sealed class AmbushDroneFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.AmbushDrone;
    public override CharacterCultureSid Culture => CharacterCultureSid.Black;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new AmbushDroneGraphicsConfig(ClassName);
    }
}