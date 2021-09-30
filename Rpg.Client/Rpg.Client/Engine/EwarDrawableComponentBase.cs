﻿using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal abstract class EwarDrawableComponentBase : Renderable
    {
        public EwarGame Game { get; protected set; }

        public void Initialize(EwarGame game)
        {
            Game = game;
            DoInitialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            var children = _children.ToList();

            foreach (var ewarDrawableComponent in children.OfType<EwarDrawableComponentBase>())
            {
                if (ewarDrawableComponent.Parent == this)
                {
                    ewarDrawableComponent.Update(gameTime);
                }
            }
        }

        protected override void AfterAddChild(Renderable child)
        {
            if (child is not EwarDrawableComponentBase ewarComponen)
            {
                return;
            }

            ewarComponen.Initialize(Game);
        }

        protected virtual void DoInitialize() { }
    }
}