﻿using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Engine.PostProcessing;

public sealed class PostEffectCatalog
{
    private Effect? _hurtEffect;
    private Effect? _shakeEffect;

    public Effect HurtEffect => _hurtEffect ?? throw new InvalidOperationException();

    public Effect ShakeEffect => _shakeEffect ?? throw new InvalidOperationException();

    public void Load(ContentManager contentManager)
    {
        _shakeEffect = contentManager.Load<Effect>("Effects/Shake");
        _hurtEffect = contentManager.Load<Effect>("Effects/Hurt");
    }
}