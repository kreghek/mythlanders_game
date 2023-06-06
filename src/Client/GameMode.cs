using System;

namespace Client;

[Flags]
public enum GameMode
{
    Demo = 1,
    Full = 2,
    Recording = 4
}