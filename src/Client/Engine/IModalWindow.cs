﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine;

internal interface IModalWindow
{
    bool IsVisible { get; }

    void Close();
    void Draw(SpriteBatch spriteBatch);
    void Show();
    void Update(GameTime gameTime, IScreenProjection screenProjection);
}