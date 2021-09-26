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
            //EwarGame? game = Game;

            //if (game != null)
            //{
            //    child = game;
            //}

            //Renderable? current = Parent;

            //while (current != null)
            //{
            //    if (current is EwarDrawableComponentBase ewarComponent)
            //    {
            //        game = ewarComponent.Game;
            //        break;
            //    }
            //}

            //if (game == null)
            //    throw new InvalidOperationException();
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