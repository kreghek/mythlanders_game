using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

public interface IPostEffect
{
    void Apply(PostEffectCatalog postEffectCatalog);
    void Update(GameTime gameTime, PostEffectCatalog postEffectCatalog);
}