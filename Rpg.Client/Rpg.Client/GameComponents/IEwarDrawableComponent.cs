using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Rpg.Client.GameComponents
{
    internal interface IEwarDrawableComponent
    {
        void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        IEnumerable<IEwarDrawableComponent> ChildComponents { get; }

        void AddComponent(IEwarDrawableComponent component);
        void RemoveComponent(IEwarDrawableComponent component);
        bool HasComponent(IEwarDrawableComponent component);

        event EventHandler<IEwarDrawableComponent> RemoveChild;
        event EventHandler<IEwarDrawableComponent> AddChild;
        event EventHandler<IEwarDrawableComponent> NeedRemove;
    }
}