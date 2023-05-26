using System.IO;

using Client.Assets.CombatMovements;
using Client.Core;

namespace Client.Assets.GraphicConfigs.Monsters;

internal abstract class MonsterGraphicConfig: UnitGraphicsConfigBase
{
    protected abstract CultureSid CultureSid { get; }
    public override string ThumbnalPath => Path.Combine(CommonConstants.PathToCharacterSprites, "Monsters", CultureSid.ToString(), GetType().Name[..^14], "Thumbnail");
}
