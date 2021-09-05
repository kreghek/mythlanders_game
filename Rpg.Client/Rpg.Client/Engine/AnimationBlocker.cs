using System;

namespace Rpg.Client.Engine
{
    public class AnimationBlocker
    {
        public void Release()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Released;
    }
}