using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Client.Engine;

public abstract class RectControlBase
{
    public abstract IReadOnlyList<Rectangle> GetRects();
}
