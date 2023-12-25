using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters.Egyptian;

internal abstract class EgyptianMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected EgyptianMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Egyptian;
}