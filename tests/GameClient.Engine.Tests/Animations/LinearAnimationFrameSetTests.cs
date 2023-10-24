﻿using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace GameClient.Engine.Tests.Animations;

[TestFixture]
public class LinearAnimationFrameSetTests
{
    [Test]
    public void Constructor_WithEmptyFrames_ThrowsArgumentException()
    {
        // Arrange
        var frames = ArraySegment<int>.Empty;
        const float FPS = 0;
        const int FRAME_WIDTH = 0;
        const int FRAME_HEIGHT = 0;
        const int TEXTURE_COLUMNS = 0;

        // Act & Assert

        var act = () =>
        {
            var _ = new LinearAnimationFrameSet(frames, FPS, FRAME_WIDTH, FRAME_HEIGHT, TEXTURE_COLUMNS);
        };

        // Assert

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GetFrameRect_NoUpdates_ReturnsCorrectRectangle()
    {
        // Arrange
        var frames = new List<int> { 0, 1, 2 };
        const float FPS = 1;
        const int FRAME_WIDTH = 1;
        const int FRAME_HEIGHT = 1;
        const int TEXTURE_COLUMNS = 2;
        var animationFrameSet = new LinearAnimationFrameSet(frames, FPS, FRAME_WIDTH, FRAME_HEIGHT, TEXTURE_COLUMNS);
        var expectedStartFrameRect = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);

        // Act
        var factFrameRect = animationFrameSet.GetFrameRect();

        // Assert
        factFrameRect.Should().Be(expectedStartFrameRect);
    }

    [Test]
    public void Update_FrameCounterExceedsFPS_IncrementsFrameListIndex()
    {
        // Arrange
        var frames = new List<int> { 0, 1 };
        const float FPS = 1;
        const int FRAME_WIDTH = 1;
        const int FRAME_HEIGHT = 1;
        const int TEXTURE_COLUMNS = 2;
        var animationFrameSet = new LinearAnimationFrameSet(frames, FPS, FRAME_WIDTH, FRAME_HEIGHT, TEXTURE_COLUMNS);
        var gameTime = new GameTime(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        const int EXPECTED_FRAME_INDEX = 1;

        // Act
        animationFrameSet.Update(gameTime);

        // Assert
        var factFrameRect = animationFrameSet.GetFrameRect();
        var expectedRect = new Rectangle(EXPECTED_FRAME_INDEX * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT);
        factFrameRect.Should().Be(expectedRect);
    }

    [Test]
    public void Update_IsLoopingTrue_FrameListIndexResetsToZero()
    {
        // Arrange
        var frames = new List<int> { 0, 1 };
        const float FPS = 1;
        const int FRAME_WIDTH = 1;
        const int FRAME_HEIGHT = 1;
        const int TEXTURE_COLUMNS = 2;
        var animationFrameSet = new LinearAnimationFrameSet(frames, FPS, FRAME_WIDTH, FRAME_HEIGHT, TEXTURE_COLUMNS)
        {
            IsLooping = true
        };
        var updateDuration = TimeSpan.FromSeconds(2);
        const int EXPECTED_FRAME_INDEX = 0;
        var gameTime = new GameTime(updateDuration, updateDuration);

        // Act
        animationFrameSet.Update(gameTime);

        // Assert
        var factFrameRect = animationFrameSet.GetFrameRect();
        var expectedRect = new Rectangle(EXPECTED_FRAME_INDEX * FRAME_WIDTH, 0, FRAME_WIDTH, FRAME_HEIGHT);
        factFrameRect.Should().Be(expectedRect);
    }
}