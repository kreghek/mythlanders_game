using Rpg.Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class ChineseMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected ChineseMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Chinese;
}