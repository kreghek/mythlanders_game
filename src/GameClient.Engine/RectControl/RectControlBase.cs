using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

public abstract class RectControlBase
{
    public abstract IReadOnlyList<Rectangle> GetRects();
}
