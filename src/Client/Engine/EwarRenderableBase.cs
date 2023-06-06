using System.Linq;

using Microsoft.Xna.Framework;

namespace Client.Engine;

internal abstract class EwarRenderableBase : Renderable
{
    public virtual void Update(GameTime gameTime)
    {
        var children = Children.ToList();

        foreach (var ewarDrawableComponent in children.OfType<EwarRenderableBase>())
        {
            if (ewarDrawableComponent.Parent == this)
            {
                ewarDrawableComponent.Update(gameTime);
            }
        }
    }

    protected override void AfterAddChild(Renderable child)
    {
        if (child is EwarRenderableBase ewarRenderable)
        {
            ewarRenderable.Initialize();
        }
    }

    protected virtual void DoInitialize() { }

    private void Initialize()
    {
        DoInitialize();
    }
}