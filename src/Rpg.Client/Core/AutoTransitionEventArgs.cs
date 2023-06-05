using System;

namespace Rpg.Client.Core
{
    internal sealed class AutoTransitionEventArgs : EventArgs
    {
        public AutoTransitionEventArgs(UnitScheme sourceScheme)
        {
            SourceScheme = sourceScheme;
        }

        internal UnitScheme SourceScheme { get; set; }
    }
}