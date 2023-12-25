using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters.Chinese;

internal abstract class ChineseMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected ChineseMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Chinese;
}