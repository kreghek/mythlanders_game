﻿using GameClient.Engine.Animations;

using MonoGame.Extended.SceneGraphs;

namespace Client.Engine;

internal interface IActorAnimator
{
    SceneNode GraphicRoot { get; }

    IAnimationFrameSet GetIdleState();

    void PlayAnimation(IAnimationFrameSet animation);
}