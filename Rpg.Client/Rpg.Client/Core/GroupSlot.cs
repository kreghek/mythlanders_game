﻿namespace Rpg.Client.Core
{
    internal sealed class GroupSlot
    {
        public int Index { get; init; }
        public bool IsTankLine { get; init; }
        public Unit? Unit { get; set; }
    }
}