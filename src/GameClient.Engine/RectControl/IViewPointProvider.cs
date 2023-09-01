using Microsoft.Xna.Framework;

namespace GameClient.Engine.RectControl;

public interface IViewPointProvider
{
    Vector2 GetWorldCoords();
}
