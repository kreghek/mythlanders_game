using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;

namespace GameClient.Engine.Tests;

internal class ParallaxRectControlTests
{
    [Test]
    public void GetRects_ReturnsCorrectNumberOfRectangles()
    {
        // Arrange
        var relativeLayerSpeeds = new[]
        {
            Vector2.Zero,
            Vector2.Zero
        };

        var parallaxRectControl = new ParallaxRectControl(
            Rectangle.Empty, // parentRectangle
            Rectangle.Empty, // layerRectangle
            relativeLayerSpeeds,
            Mock.Of<IParallaxViewPointProvider>());

        var expectedCount = relativeLayerSpeeds.Length;

        // Act

        var rects = parallaxRectControl.GetRects();

        // Assert
        rects.Should().HaveCount(expectedCount);
    }

    [Test]
    public void GetRects_ReturnsRectanglesWithCorrectPositions()
    {
        // Arrange
        var expectedPositions = new Vector2[]
        {
            new(-1000.0f, -750.0f),
            new(-1200.0f, -900)
        };

        var parentRectangle = new Rectangle(0, 0, 800, 600);

        // IViewPointProvider mock returns fixed values
        var viewPointProviderMock = new Mock<IParallaxViewPointProvider>();

        viewPointProviderMock.Setup(vpp => vpp.GetWorldCoords())
            .Returns(parentRectangle.Center.ToVector2() + new Vector2(400f, 300f));

        var layerRelativeSpeeds = new[]
        {
            new Vector2(0.5f, 0.5f),
            new Vector2(1.0f, 1.0f)
        };

        var parallaxRectControl = new ParallaxRectControl(
            parentRectangle, // parentRectangle
            new Rectangle(0, 0, 1600, 1200), // layerRectangle
            layerRelativeSpeeds,
            viewPointProviderMock.Object // viewPointProvider
        );

        // Act
        var rects = parallaxRectControl.GetRects();

        // Assert

        var rectPositions = rects.Select(x => x.Location.ToVector2());
        rectPositions.Should().BeEquivalentTo(expectedPositions);
    }

    [Test]
    public void GetRects_ViewPointMoves_RectsMovesToViewPoint()
    {
        // ARRANGE

        var provider = Mock.Of<IParallaxViewPointProvider>(x => x.GetWorldCoords() == new Vector2(6, 5));

        var screenRect = new Rectangle(0, 0, 10, 10);
        var layerRect = new Rectangle(0, 0, 20, 10); // will placed on -10
        var relativeSpeeds = new[]
        {
            new Vector2(1, 0)
        };
        var rectControl = new ParallaxRectControl(screenRect, layerRect, relativeSpeeds, provider);

        // ACT

        var rects = rectControl.GetRects();

        // ASSERT

        rects[0].X.Should().Be(-11);
    }
}