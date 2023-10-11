using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Client.Engine.PostProcessing;

public sealed class PostEffectManager
{
    private readonly PostEffectCatalog _postEffectCatalog;
    private readonly IList<IPostEffect> _postList = new List<IPostEffect>();

    public PostEffectManager(PostEffectCatalog postEffectCatalog)
    {
        _postEffectCatalog = postEffectCatalog;
    }

    public void AddEffect(IPostEffect postEffect)
    {
        _postList.Add(postEffect);
    }

    public void Apply()
    {
        foreach (var effect in _postList.ToArray())
        {
            effect.Apply(_postEffectCatalog);
        }
    }

    public void RemoveEffect(IPostEffect postEffect)
    {
        _postList.Remove(postEffect);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var effect in _postList.ToArray())
        {
            effect.Update(gameTime, _postEffectCatalog);
        }
    }
}