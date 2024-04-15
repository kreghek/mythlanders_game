using Client.Assets.GraphicConfigs.Monsters;
using Client.Assets.GraphicConfigs.Monsters.Slavic;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class CorruptedBearFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.CorruptedBear;

    public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new CorruptedBearConfig(ClassName);
    }
}