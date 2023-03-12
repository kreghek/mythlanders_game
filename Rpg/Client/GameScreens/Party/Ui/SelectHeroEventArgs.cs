using System;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal sealed class SelectHeroEventArgs : EventArgs
    {
        public SelectHeroEventArgs(Core.Heroes.Hero character)
        {
            Character = character;
        }

        public Core.Heroes.Hero Character { get; }
    }
}