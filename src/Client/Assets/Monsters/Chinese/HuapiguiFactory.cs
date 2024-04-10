using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Chinese;

[UsedImplicitly]
internal sealed class HuapiguiFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.Huapigui;

    public override CharacterCultureSid Culture => CharacterCultureSid.Chinese;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}