using Rpg.Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class EgyptianMonsterGraphicConfig : MonsterGraphicsConfig
{
    protected EgyptianMonsterGraphicConfig(UnitName unit) : base(unit)
    {
    }

    protected override CharacterCultureSid CultureSid => CharacterCultureSid.Egyptian;
}