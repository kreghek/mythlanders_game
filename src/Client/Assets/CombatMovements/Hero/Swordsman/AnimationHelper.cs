using Client.Core;
using Client.Core.AnimationFrameSets;
using Client.Engine;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal static class AnimationHelper
{
    public static IAnimationFrameSet ConvertToAnimation(SpriteAtlasAnimationData spredsheetAnimationData,
        string animation)
    {
        var spredsheetAnimationDataCycles = spredsheetAnimationData.Cycles[animation];

        return new LinearAnimationFrameSet(
            spredsheetAnimationDataCycles.Frames,
            spredsheetAnimationDataCycles.Fps,
            spredsheetAnimationData.TextureAtlas.RegionWidth,
            spredsheetAnimationData.TextureAtlas.RegionHeight, 8)
        {
            IsLooping = spredsheetAnimationDataCycles.IsLooping
        };
    }
}