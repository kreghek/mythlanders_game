using Client.Engine;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class ActorAnimator : IActorAnimator
{
    private readonly UnitGraphics _unitGraphics;

    public SpriteContainer GraphicRoot { get; }

    public ActorAnimator(UnitGraphics unitGraphics)
    {
        GraphicRoot = unitGraphics.Root;
        _unitGraphics = unitGraphics;
    }

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