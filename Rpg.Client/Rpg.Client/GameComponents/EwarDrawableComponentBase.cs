using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameComponents
{
    internal abstract class EwarDrawableComponentBase : IEwarDrawableComponent
    {
        private readonly Queue<IEwarDrawableComponent> _componentsToRemove;

        protected EwarDrawableComponentBase(EwarGame game)
        {
            Game = game;
            ComponentsCollection = new List<IEwarDrawableComponent>();
            _componentsToRemove = new Queue<IEwarDrawableComponent>();
        }

        protected ICollection<IEwarDrawableComponent> ComponentsCollection { get; }
        protected EwarGame Game { get; }
        protected SpriteBatch SpriteBatch { get; }

        protected void Remove()
        {
            NeedRemove?.Invoke(this, this);
        }

        private void Component_NeedRemove(object? sender, IEwarDrawableComponent e)
        {
            RemoveComponent(e);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var ewarDrawableComponent in ChildComponents)
            {
                ewarDrawableComponent.Draw(gameTime, spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var ewarDrawableComponent in ChildComponents)
            {
                ewarDrawableComponent.Update(gameTime);
            }

            while (_componentsToRemove.Count > 0)
            {
                ComponentsCollection.Remove(_componentsToRemove.Dequeue());
            }
        }

        public IEnumerable<IEwarDrawableComponent> ChildComponents => ComponentsCollection;

        public void AddComponent(IEwarDrawableComponent component)
        {
            component.NeedRemove += Component_NeedRemove;
            ComponentsCollection.Add(component);
            AddChild?.Invoke(this, component);
        }

        public void RemoveComponent(IEwarDrawableComponent component)
        {
            component.NeedRemove -= Component_NeedRemove;
            _componentsToRemove.Enqueue(component);
            RemoveChild?.Invoke(this, component);
        }

        public bool HasComponent(IEwarDrawableComponent component)
        {
            return ComponentsCollection.Contains(component);
        }

        public event EventHandler<IEwarDrawableComponent>? RemoveChild;
        public event EventHandler<IEwarDrawableComponent>? AddChild;
        public event EventHandler<IEwarDrawableComponent>? NeedRemove;
    }
}