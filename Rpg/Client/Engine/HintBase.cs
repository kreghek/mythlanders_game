﻿using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal abstract class HintBase : ControlBase
    {
        protected override Color CalculateColor()
        {
            return Color.Lerp(Color.Transparent, Color.SlateBlue, 0.75f);
        }
    }
}