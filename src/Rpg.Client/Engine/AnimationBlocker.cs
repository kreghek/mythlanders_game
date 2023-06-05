using System;

namespace Rpg.Client.Engine
{
    internal class AnimationBlocker
    {
        public bool IsReleased { get; private set; }

        public void Release()
        {
            IsReleased = true;
            Released?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? Released;
    }
}