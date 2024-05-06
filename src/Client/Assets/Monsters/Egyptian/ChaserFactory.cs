using Client.Assets.GraphicConfigs.Monsters;
using Client.Assets.GraphicConfigs.Monsters.Egyptian;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Egyptian;

[UsedImplicitly]
internal sealed class ChaserFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Chaser;

    public override CharacterCultureSid Culture => CharacterCultureSid.Egyptian;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new ChaserGraphicConfig();
    }
}