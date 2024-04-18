using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

namespace Client.Assets.Monsters.Chinese;

[UsedImplicitly]
internal sealed class PaintedSkinFactory : MonsterFactoryBase
{
    public override UnitName ClassName => UnitName.PaintedSkin;

    public override CharacterCultureSid Culture => CharacterCultureSid.Chinese;

    public override CombatantGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new GenericMonsterGraphicsConfig(ClassName, Culture);
    }
}