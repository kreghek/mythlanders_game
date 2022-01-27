using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal sealed class SelectHeroEventArgs : EventArgs
    {
        public SelectHeroEventArgs(Unit character)
        {
            Character = character;
        }

        public Unit Character { get; }
    }
}