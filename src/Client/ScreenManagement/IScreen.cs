﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.ScreenManagement;

internal interface IScreen
{
    IScreen? TargetScreen { get; set; }

    void Draw(SpriteBatch spriteBatch);

    void Update(GameTime gameTime);
}