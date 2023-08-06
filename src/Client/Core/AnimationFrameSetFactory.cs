using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.AnimationFrameSets;

namespace Client.Core;

internal static class AnimationFrameSetFactory
{
    public static IAnimationFrameSet CreateIdle(int startFrameIndex = 0, int frameCount = 8,
        int fps = 8, int frameWidth = 256,
        int frameHeight = 128, int textureColumns = 8)
    {
        var frames = Enumerable.Range(startFrameIndex, frameCount).ToList();
        return new LinearAnimationFrameSet(frames, fps,
            frameWidth, frameHeight, textureColumns)
        {
            IsLooping = true,
            IsIdle = true
        };
    }

    public static IAnimationFrameSet CreateIdleFromGrid(IReadOnlyList<int> rows,
        int fps = 8, int frameWidth = 256,
        int frameHeight = 128, int textureColumns = 8)
    {
        var frames = new List<int>();

        foreach (var row in rows)
        {
            var startFrameIndex = row * textureColumns;
            var rowFrames = Enumerable.Range(startFrameIndex, textureColumns).ToList();
            frames.AddRange(rowFrames);
        }

        return new LinearAnimationFrameSet(frames, fps,
            frameWidth, frameHeight, textureColumns)
        {
            IsLooping = true,
            IsIdle = true
        };
    }

    public static IAnimationFrameSet CreateSequential(int startFrameIndex, int frameCount, float fps,
        int frameWidth = 256,
        int frameHeight = 128, int textureColumns = 8, bool isLoop = false)
    {
        var frames = Enumerable.Range(startFrameIndex, frameCount).ToList();
        return new LinearAnimationFrameSet(frames, fps,
            frameWidth, frameHeight, textureColumns)
        {
            IsLooping = isLoop
        };
    }

    public static IAnimationFrameSet CreateSequentialFromGrid(IReadOnlyList<int> rows, float fps,
        int frameWidth = 256,
        int frameHeight = 128, int textureColumns = 8, bool isLoop = false)
    {
        var frames = new List<int>();

        foreach (var row in rows)
        {
            var startFrameIndex = row * textureColumns;
            var rowFrames = Enumerable.Range(startFrameIndex, textureColumns).ToList();
            frames.AddRange(rowFrames);
        }

        return new LinearAnimationFrameSet(frames, fps,
            frameWidth, frameHeight, textureColumns)
        {
            IsLooping = isLoop
        };
    }
}