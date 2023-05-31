using System.IO;

using Client.Assets.CombatMovements;
using Client.Core;

namespace Client.Assets.GraphicConfigs.Heroes;

internal abstract class HeroGraphicConfig : UnitGraphicsConfigBase
{
    public override string ThumbnailPath => Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes",
        GetType().Name[..^14], "Thumbnail");
}