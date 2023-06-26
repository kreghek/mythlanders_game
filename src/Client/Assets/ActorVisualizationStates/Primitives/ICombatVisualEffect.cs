using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.ActorVisualizationStates.Primitives;

interface ICombatVisualEffect
{
    void Update(GameTime gameTime);
    void DrawBack(SpriteBatch spriteBatch);
    void DrawFront(SpriteBatch spriteBatch);

    bool IsDestroyed { get; }
}