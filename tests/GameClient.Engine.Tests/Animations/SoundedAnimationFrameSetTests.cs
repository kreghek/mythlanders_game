using GameClient.Engine.Animations;

namespace GameClient.Engine.Tests.Animations;

public class SoundedAnimationFrameSetTests
{
    [Test]
    public void KeyFrame_BaseRaisesEvent_EventRaisedOnce()
    {
        // ARRANGE

        var baseAnimationMock = new Mock<IAnimationFrameSet>();

        var animation = new SoundedAnimationFrameSet(baseAnimationMock.Object,
            ArraySegment<AnimationFrame<IAnimationSoundEffect>>.Empty);

        var raiseCount = 0;
        animation.KeyFrame += (_, _) =>
        {
            raiseCount++;
        };

        // ACT

        baseAnimationMock.Raise(x => x.KeyFrame += null!, new AnimationFrameEventArgs(Mock.Of<IAnimationFrameInfo>()));

        // ASSERT

        raiseCount.Should().Be(1);
    }
}