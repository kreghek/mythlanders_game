using Microsoft.Xna.Framework;

namespace Client.Engine;

public interface IMoveFunction
{
    Vector2 CalcPosition(double t);
}