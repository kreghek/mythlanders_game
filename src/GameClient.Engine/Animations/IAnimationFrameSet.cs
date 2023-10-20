using Microsoft.Xna.Framework;

namespace GameClient.Engine.Animations;

public interface IAnimationFrameSet
{
    bool IsIdle { get; }
    Rectangle GetFrameRect();
    void Reset();
    void Update(GameTime gameTime);
    event EventHandler? End;
    event EventHandler<KeyFrameEventArgs> KeyFrame;
}