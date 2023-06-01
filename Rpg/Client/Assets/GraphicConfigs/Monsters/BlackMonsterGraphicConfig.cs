using Rpg.Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class BlackMonsterGraphicConfig : MonsterGraphicConfig
{
    protected BlackMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Black;
}