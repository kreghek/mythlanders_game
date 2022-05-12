using System.Linq;

using Rpg.Client.Core.AnimationFrameSets;

namespace Rpg.Client.Core
{
    internal static class AnimationFrameSetFactory
    {
        public static IAnimationFrameSet CreateSequential(int startFrameIndex, int frameCount, int speedMultiplicator, int frameWidth = 256,
            int frameHeight = 128, int textureColumns = 8, bool isLoop = false)
        {
            var frames = Enumerable.Range(startFrameIndex, frameCount).ToList();
            return new SequentalAnimationFrameSet(frames, speedMultiplicator,
                frameWidth, frameHeight, textureColumns)
            {
                IsLoop = isLoop
            };
        }

        public static IAnimationFrameSet CreateIdle(int startFrameIndex = 0, int frameCount = 8, int speedMultiplicator = 8, int frameWidth = 256,
            int frameHeight = 128, int textureColumns = 8)
        {
            var frames = Enumerable.Range(startFrameIndex, frameCount).ToList();
            return new SequentalAnimationFrameSet(frames, speedMultiplicator,
                frameWidth, frameHeight, textureColumns)
            {
                IsLoop = true,
                IsIdle = true
            };
        }
    }
}