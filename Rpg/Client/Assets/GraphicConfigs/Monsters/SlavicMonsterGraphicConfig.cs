using Rpg.Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class SlavicMonsterGraphicConfig : MonsterGraphicConfig
{
    protected SlavicMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Slavic;
}