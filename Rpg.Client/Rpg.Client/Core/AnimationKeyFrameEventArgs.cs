using System;

namespace Rpg.Client.Core
{
    internal sealed class AnimationKeyFrameEventArgs : EventArgs {
        public AnimationKeyFrameEventArgs(IAnimationKeyFrame keyFrame)
        {
            KeyFrame = keyFrame ?? throw new ArgumentNullException(nameof(keyFrame));
        }

        public IAnimationKeyFrame KeyFrame { get; }
    }
}