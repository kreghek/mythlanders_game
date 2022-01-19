using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal sealed class SelectCharacterEventArgs : EventArgs
    {
        public SelectCharacterEventArgs(Unit character)
        {
            Character = character;
        }

        public Unit Character { get; }
    }
}