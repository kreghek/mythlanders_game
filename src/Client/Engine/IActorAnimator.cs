using Client.Core;

using Microsoft.Xna.Framework;

namespace Client.Engine;

internal interface IActorAnimator
{
    SpriteContainer GraphicRoot { get; }

    IAnimationFrameSet GetIdleState();

    void PlayAnimation(IAnimationFrameSet animation);

    void Update(GameTime gameTime);
}