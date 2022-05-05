﻿using System;

namespace Rpg.Client.Core
{
    public class Stat
    {
        public Stat(int baseValue)
        {
            Base = baseValue;
            Current = Base;
        }

        public int ActualBase => Base;

        public int Current { get; private set; }

        public float Share => (float)Current / ActualBase;

        private int Base { get; set; }

        public void ChangeBase(int newBase)
        {
            Base = newBase;
            Current = newBase;
        }

        public void Consume(int value)
        {
            Current -= value;

            if (Current < 0)
            {
                Current = 0;
            }
        }

        public void CurrentChange(int newCurrent)
        {
            Current = Math.Min(newCurrent, ActualBase);
        }

        public void Restore(int value)
        {
            Current += value;

            if (Current > Base)
            {
                Current = Base;
            }
        }

        internal void Restore()
        {
            Current = ActualBase;
        }
    }
}