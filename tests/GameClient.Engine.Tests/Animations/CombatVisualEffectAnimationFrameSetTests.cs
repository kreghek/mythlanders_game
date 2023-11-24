using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

namespace GameClient.Engine.Tests.Animations;

public class CombatVisualEffectAnimationFrameSetTests
{
    [Test]
    public void KeyFrame_BaseRaisesEvent_EventRaisedOnce()
    {
        // ARRANGE

        var baseAnimationMock = new Mock<IAnimationFrameSet>();

        var animation = new CombatVisualEffectAnimationFrameSet(baseAnimationMock.Object,
            Mock.Of<ICombatVisualEffectManager>(), Array.Empty<AnimationFrame<ICombatVisualEffect>>());

        var raiseCount = 0;
        animation.KeyFrame += (_, _) =>
        {
            raiseCount++;
        };

        // ACT
        
        baseAnimationMock.Raise(x=>x.KeyFrame += null!, new AnimationFrameEventArgs(Mock.Of<IAnimationFrameInfo>()));
        
        // ASSERT

        raiseCount.Should().Be(1);
    }
}