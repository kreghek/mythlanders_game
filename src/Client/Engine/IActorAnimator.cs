using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Client.Engine;

internal interface IActorAnimator
{
    SpriteContainer GraphicRoot { get; }

    IAnimationFrameSet GetIdleState();

    void PlayAnimation(IAnimationFrameSet animation);

    void Update(GameTime gameTime);
}