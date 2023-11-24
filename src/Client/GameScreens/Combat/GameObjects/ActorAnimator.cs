using Client.Core;
using Client.Engine;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

using MonoGame.Extended.SceneGraphs;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class ActorAnimator : IActorAnimator
{
    private readonly UnitGraphics _unitGraphics;

    public ActorAnimator(UnitGraphics unitGraphics)
    {
        GraphicRoot = unitGraphics.Root.RootNode;
        _unitGraphics = unitGraphics;
    }

    public SceneNode GraphicRoot { get; }

    public IAnimationFrameSet GetIdleState()
    {
        return _unitGraphics.GetAnimationInfo(PredefinedAnimationSid.Idle);
    }

    public void PlayAnimation(IAnimationFrameSet animation)
    {
        _unitGraphics.PlayAnimation(animation);
    }
}