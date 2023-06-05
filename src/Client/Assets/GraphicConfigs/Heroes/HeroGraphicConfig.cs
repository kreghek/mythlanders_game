using System.IO;

using Client.Assets.CombatMovements;
using Client.Core;

using Rpg.Client.Core;

namespace Client.Assets.GraphicConfigs.Heroes;

internal abstract class HeroGraphicConfig : UnitGraphicsConfigBase
{
    private readonly UnitName _name;

    protected HeroGraphicConfig(UnitName name)
    {
        _name = name;
    }

    public override string ThumbnailPath => Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes",
        _name.ToString(), "Thumbnail");
}