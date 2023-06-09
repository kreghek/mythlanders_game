using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class ActorAnimator : IActorAnimator
{
    private readonly UnitGraphics _unitGraphics;

    public ActorAnimator(UnitGraphics unitGraphics)
    {
        GraphicRoot = unitGraphics.Root;
        _unitGraphics = unitGraphics;
    }

    public SpriteContainer GraphicRoot { get; }

    public IAnimationFrameSet GetIdleState()
    {
        return _unitGraphics.GetAnimationInfo(PredefinedAnimationSid.Idle);
    }

    public void PlayAnimation(IAnimationFrameSet animation)
    {
        _unitGraphics.PlayAnimation(animation);
    }

    public void Update(GameTime gameTime)
    {
    }
}