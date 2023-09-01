using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

public interface IPongRectangleRandomSource
{
    Vector2 GetRandomVector();
}