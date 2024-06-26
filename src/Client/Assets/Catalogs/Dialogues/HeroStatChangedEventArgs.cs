﻿using System;

namespace Client.Assets.Catalogs.Dialogues;

public sealed class HeroStatChangedEventArgs : EventArgs
{
    public HeroStatChangedEventArgs(string hero, int amount)
    {
        Hero = hero;
        Amount = amount;
    }

    public int Amount { get; }

    public string Hero { get; }
}