using System;

using Microsoft.Xna.Framework;

namespace Client.Core;

internal interface IAnimationFrameSet
{
    bool IsIdle { get; }
    Rectangle GetFrameRect();
    void Reset();
    void Update(GameTime gameTime);
    event EventHandler? End;
}