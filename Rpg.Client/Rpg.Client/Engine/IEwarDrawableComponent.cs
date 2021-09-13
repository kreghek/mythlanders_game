using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal interface IEwarDrawableComponent
    {
        IEnumerable<IEwarDrawableComponent> ChildComponents { get; }

        void AddComponent(IEwarDrawableComponent component);
        void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
        bool HasComponent(IEwarDrawableComponent component);
        void RemoveComponent(IEwarDrawableComponent component);
        void Update(GameTime gameTime);

        event EventHandler<IEwarDrawableComponent> RemoveChild;
        event EventHandler<IEwarDrawableComponent> AddChild;
        event EventHandler<IEwarDrawableComponent> NeedRemove;
    }
}