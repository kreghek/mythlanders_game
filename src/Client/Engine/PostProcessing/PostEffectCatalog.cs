using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine.PostProcessing;

public sealed class PostEffectCatalog
{
    private Effect? _shakeEffect;

    public Effect ShakeEffect => _shakeEffect ?? throw new InvalidOperationException();

    public void Load(ContentManager contentManager)
    {
        _shakeEffect = contentManager.Load<Effect>("Effects/Shake");
    }
}