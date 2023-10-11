using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

public sealed class HurtPostEffect : IPostEffect
{
    public void Apply(PostEffectCatalog postEffectCatalog)
    {
        foreach (var technique in postEffectCatalog.HurtEffect.Techniques)
        {
            foreach (var pass in technique.Passes)
            {
                pass.Apply();
            }
        }
    }

    public void Update(GameTime gameTime, PostEffectCatalog postEffectCatalog)
    {
        
    }
}