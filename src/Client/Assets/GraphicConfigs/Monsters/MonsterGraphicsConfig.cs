using System.IO;

using Client.Assets.CombatMovements;
using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class MonsterGraphicsConfig : UnitGraphicsConfigBase
{
    private readonly UnitName _unit;

    public MonsterGraphicsConfig(UnitName unit)
    {
        _unit = unit;
    }

    public override string ThumbnailPath => Path.Combine(CommonConstants.PathToCharacterSprites, "Monsters",
        CultureSid.ToString(), _unit.ToString(), "Thumbnail");

    protected abstract CharacterCultureSid CultureSid { get; }
}