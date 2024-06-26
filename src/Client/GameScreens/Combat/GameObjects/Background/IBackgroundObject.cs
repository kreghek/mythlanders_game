﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects.Background;

internal interface IBackgroundObject
{
    void Draw(SpriteBatch spriteBatch);
    void Update(GameTime gameTime);
}