using System;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;

using FluentAssertions;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Assets.ActorVisualizationStates;

public class LaunchAndWaitInteractionDeliveryStateTests
{
    [Test]
    public void Update_AnimationWithSingleFrame_RegisterInteractionDeliveryOnce()
    {
        // ARRANGE

        var launchAnimationMock = new Mock<IAnimationFrameSet>();

        var deliveryManager = new InteractionDeliveryManager();

        var launchAnimationKeyFrame = new AnimationFrameInfo(0);

        var _ = new LaunchAndWaitInteractionDeliveryState(Mock.Of<IActorAnimator>(), launchAnimationMock.Object,
            Mock.Of<IAnimationFrameSet>(),
            new[]
            {
                new InteractionDeliveryInfo(new CombatEffectImposeItem(_ => { }, ArraySegment<ICombatant>.Empty),
                    Vector2.One, Vector2.One)
            },
            Mock.Of<IDeliveryFactory>(x =>
                x.Create(It.IsAny<CombatEffectImposeItem>(), It.IsAny<Vector2>(), It.IsAny<Vector2>()) ==
                Mock.Of<IInteractionDelivery>()),
            deliveryManager, launchAnimationKeyFrame);

        // ACT

        launchAnimationMock.Raise(x => x.KeyFrame += null!, new AnimationFrameEventArgs(launchAnimationKeyFrame));

        // ASSERT

        deliveryManager.GetActiveSnapshot().Should().HaveCount(1);
    }
}