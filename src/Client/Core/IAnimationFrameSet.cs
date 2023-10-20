using System;

using Microsoft.Xna.Framework;

namespace Client.Core;

// CS0552.cs
internal interface IAnimationFrameSet
{
    bool IsIdle { get; }
    Rectangle GetFrameRect();
    void Reset();
    void Update(GameTime gameTime);
    event EventHandler? End;
    event EventHandler<KeyFrameEventArgs> KeyFrame;
}