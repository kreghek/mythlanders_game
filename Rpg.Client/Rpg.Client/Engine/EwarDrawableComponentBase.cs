using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal abstract class EwarDrawableComponentBase : Renderable
    {
        protected override void AfterAddChild(Renderable child)
        {
            if (child is not EwarDrawableComponentBase ewarComponen)
                return;
            
            ewarComponen.Initialize(Game);
        }

        public void Initialize(EwarGame game)
        {
            Game = game;
            DoInitialize();
        }

        protected virtual void DoInitialize() {}

        public EwarGame Game { get; protected set; }

        public virtual void Update(GameTime gameTime)
        {
            var children = _children.ToList();

            foreach (var ewarDrawableComponent in children.OfType<EwarDrawableComponentBase>())
            {
                if (ewarDrawableComponent.Parent == this)
                    ewarDrawableComponent.Update(gameTime);
            }
        }
    }
}