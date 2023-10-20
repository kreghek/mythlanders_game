using System;

namespace Client.Core;

internal sealed class KeyFrameEventArgs : EventArgs
{
    public KeyFrameEventArgs(int frameIndex)
    {
        FrameIndex = frameIndex;
    }
    public int FrameIndex { get; }
}