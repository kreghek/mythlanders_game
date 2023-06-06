using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class BlackMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected BlackMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Black;
}