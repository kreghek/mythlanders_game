using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal abstract class EwarDrawableComponentBase : Renderable
    {
        public virtual void Update(GameTime gameTime)
        {
            var children = Children.ToList();

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

            ewarComponen.Initialize();
        }

        protected virtual void DoInitialize() { }

        private void Initialize()
        {
            DoInitialize();
        }
    }
}