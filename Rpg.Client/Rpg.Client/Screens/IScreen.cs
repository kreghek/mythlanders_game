﻿using Microsoft.Xna.Framework;

namespace Rpg.Client.Screens
{
    internal interface IScreen
    {
        IScreen? TargetScreen { get; set; }
        void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}