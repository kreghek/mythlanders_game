namespace GameClient.Engine.Animations;

public sealed class KeyFrameEventArgs : EventArgs
{
    public KeyFrameEventArgs(int frameIndex)
    {
        FrameIndex = frameIndex;
    }
    public int FrameIndex { get; }
}