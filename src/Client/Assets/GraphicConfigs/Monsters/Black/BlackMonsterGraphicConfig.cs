using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters.Black;

internal abstract class BlackMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected BlackMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Black;
}