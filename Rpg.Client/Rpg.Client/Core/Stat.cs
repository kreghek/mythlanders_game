using System;

namespace Rpg.Client.Core
{
    public class Stat
    {
        public Stat(int baseValue)
        {
            Base = baseValue;
            Current = Base;
        }

        private int Base { get; set; }

        public void ChangeBase(int newBase)
        {
            Base = newBase;
            Current = newBase;
        }

        public int ActualBase => Base;

        public int Current { get; private set; }

        public void Increase(int value)
        {
            Current += value;

            if (Current > Base)
            {
                Current = Base;
            }
        }

        public void Descrease(int value)
        {
            Current -= value;

            if (Current < 0)
            {
                Current = 0;
            }
        }

        public float Share => (float)Current / ActualBase;

        internal void Restore()
        {
            Current = ActualBase;
        }
    }
}