using Client.Core;
using Client.Engine;

using GameClient.Engine.Animations;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Auxiliary class to group methods to work with animations.
/// </summary>
internal static class AnimationHelper
{
    /// <summary>
    /// Convert animation from content to game engine animation.
    /// </summary>
    /// <param name="spredsheetAnimationData">Content animation.</param>
    /// <param name="animation">Animation name.</param>
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