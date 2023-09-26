using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;

namespace GameClient.Engine.Tests;

internal class ParallaxRectControlTests
{
    [Test]
    public void GetRects_ReturnsCorrectNumberOfRectangles()
    {
        // Arrange

        var parallaxRectControl = new ParallaxRectControl(
            Rectangle.Empty, // parentRectangle
            new[] { Point.Zero, Point.Zero }, // layerRectangle
            Mock.Of<IParallaxViewPointProvider>());

        var expectedCount = 2;

        // Act

        var rects = parallaxRectControl.GetRects();

        // Assert
        rects.Should().HaveCount(expectedCount);
    }

    [Test]
    public void GetRects_CursorOnCenter_ReturnsRectanglesWithCorrectPositions()
    {
        // Arrange
        var expectedPositions = new Vector2[]
        {
            new Rectangle(0, 0, 1600- 800, 1200 - 600).Center.ToVector2() * -1
        };

        var parentRectangle = new Rectangle(0, 0, 800, 600);

        // IViewPointProvider mock returns fixed values
        var viewPointProviderMock = new Mock<IParallaxViewPointProvider>();

        viewPointProviderMock.Setup(vpp => vpp.GetWorldCoords())
            .Returns(parentRectangle.Center.ToVector2());

        var parallaxRectControl = new ParallaxRectControl(
            parentRectangle, // parentRectangle
            new[] { new Point(1600, 1200) }, // layerRectangle
            viewPointProviderMock.Object // viewPointProvider
        );

        // Act
        var rects = parallaxRectControl.GetRects();

        // Assert

        var rectPositions = rects.Select(x => x.Location.ToVector2());
        rectPositions.Should().BeEquivalentTo(expectedPositions);
    }

    [Test]
    public void GetRects_CursorOnLeft_ReturnsRectanglesWithCorrectPositions()
    {
        // Arrange

        var parentRectangle = new Rectangle(0, 0, 800, 600);

        // IViewPointProvider mock returns fixed values
        var viewPointProviderMock = new Mock<IParallaxViewPointProvider>();

        viewPointProviderMock.Setup(vpp => vpp.GetWorldCoords())
            .Returns(new Vector2(0, 0));

        var parallaxRectControl = new ParallaxRectControl(
            parentRectangle, // parentRectangle
            new[] { new Point(1600, 1200) }, // layerRectangle
            viewPointProviderMock.Object // viewPointProvider
        );

        // Act
        var rects = parallaxRectControl.GetRects();

        // Assert

        rects[0].Left.Should().Be(parentRectangle.Left);
    }

    [Test]
    public void GetRects_CursorOnRight_ReturnsRectanglesWithCorrectPositions()
    {
        // Arrange

        var parentRectangle = new Rectangle(0, 0, 800, 600);

        // IViewPointProvider mock returns fixed values
        var viewPointProviderMock = new Mock<IParallaxViewPointProvider>();

        viewPointProviderMock.Setup(vpp => vpp.GetWorldCoords())
            .Returns(new Vector2(800f, 600f));

        var parallaxRectControl = new ParallaxRectControl(
            parentRectangle, // parentRectangle
            new[] { new Point(1600, 1200) }, // layerRectangle
            viewPointProviderMock.Object // viewPointProvider
        );

        // Act
        var rects = parallaxRectControl.GetRects();

        // Assert

        rects[0].Right.Should().Be(parentRectangle.Right);
    }

    [Test]
    public void GetRects_SizesEqual_RectsDoNotMoves()
    {
        // Arrange

        var parentRectangle = new Rectangle(0, 0, 800, 600);

        // IViewPointProvider mock returns fixed values
        var viewPointProviderMock = new Mock<IParallaxViewPointProvider>();

        viewPointProviderMock.Setup(vpp => vpp.GetWorldCoords())
            .Returns(new Vector2(800f, 600f));

        var parallaxRectControl = new ParallaxRectControl(
            parentRectangle, // parentRectangle
            new[] { new Point(800, 600) }, // layerRectangle
            viewPointProviderMock.Object // viewPointProvider
        );

        // Act
        var rects = parallaxRectControl.GetRects();

        // Assert

        rects[0].Location.Should().Be(Point.Zero);
    }
}