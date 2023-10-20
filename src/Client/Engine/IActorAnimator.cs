﻿using Client.Core;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

using MonoGame.Extended.SceneGraphs;

namespace Client.Engine;

internal interface IActorAnimator
{
    SceneNode GraphicRoot { get; }

    IAnimationFrameSet GetIdleState();

    void PlayAnimation(IAnimationFrameSet animation);

    void Update(GameTime gameTime);
}