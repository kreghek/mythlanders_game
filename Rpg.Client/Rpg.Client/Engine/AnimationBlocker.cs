using System;

namespace Rpg.Client.Engine
{
    internal class AnimationBlocker
    {
        public void Release()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Released;
    }
}