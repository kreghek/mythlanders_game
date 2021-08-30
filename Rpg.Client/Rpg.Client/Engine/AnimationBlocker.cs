using System;

namespace Rpg.Client.Engine
{
    internal class AnimationBlocker
    {
        public event EventHandler Released;
        public void Release()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }
    }
}
