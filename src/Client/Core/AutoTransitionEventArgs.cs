using System;

namespace Client.Core;

internal sealed class AutoTransitionEventArgs : EventArgs
{
    public AutoTransitionEventArgs(UnitScheme sourceScheme)
    {
        SourceScheme = sourceScheme;
    }

    internal UnitScheme SourceScheme { get; set; }
}