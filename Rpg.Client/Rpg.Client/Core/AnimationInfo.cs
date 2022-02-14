﻿namespace Rpg.Client.Core
{
    internal record AnimationInfo
    {
        public AnimationInfo(int startFrame, int frames, float speed)
        {
            StartFrame = startFrame;
            Frames = frames;
            Speed = speed;
        }

        public int Frames { get; }

        public bool IsFinal { get; init; }
        public float Speed { get; }

        public int StartFrame { get; }

        public double GetDuration()
        {
            return 1 / Speed * Frames;
        }
    }
}