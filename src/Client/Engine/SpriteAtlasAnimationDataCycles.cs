namespace Client.Engine;

/// <summary>
/// Dto to load animations from file.
/// </summary>
internal record SpriteAtlasAnimationDataCycles(int[] Frames, float FrameDuration, bool IsLooping)
{
    public float Fps => 1f / FrameDuration;
}