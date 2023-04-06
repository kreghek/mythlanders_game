using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects;

internal interface IInteractionDelivery
{
    bool IsDestroyed { get; }

    void Draw(SpriteBatch spriteBatch);
    void Update(GameTime gameTime);

    event EventHandler? InteractionPerformed;
}