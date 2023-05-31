namespace Client.Engine;

internal record SpredsheetAnimationDataCycles(int[] Frames, float FrameDuration) { public float Fps => 1f / FrameDuration; }
