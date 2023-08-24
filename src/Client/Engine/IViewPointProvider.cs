using Microsoft.Xna.Framework;

namespace Client.Engine;

public interface IViewPointProvider
{
    Vector2 GetWorldCoords();
}
