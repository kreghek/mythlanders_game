namespace Client.Engine;

internal record SpredsheetAnimationDataCycles(int[] Frames, float FrameDuration, bool IsLooping) { public float Fps => 1f / FrameDuration; }
