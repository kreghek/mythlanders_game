using Client.Engine;

using FluentAssertions;

using Microsoft.Xna.Framework;

using NUnit.Framework;

namespace Client.Tests.Engine;

[TestFixture]
public class LineHelperTests
{
    [Test]
    public void GetBrokenLine_Simpliest()
    {
        // ASSERT
        
        var start = new Point(0, 0);
        var end = new Point(2, 2);
        var opt = new LineHelper.BrokenLineOptions { MinimalMargin = 1 };

        var expected = new[]
        {
            new Point(0, 0),
            new Point(0, 1),
            new Point(1, 2),
            new Point(2, 2)
        };
        
        // ACT

        var factPoints = LineHelper.GetBrokenLine(start.X, start.Y, end.X, end.Y, opt);
        
        // ASSERT

        factPoints.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void GetBrokenLine_Simpliest2()
    {
        // ASSERT
        
        var start = new Point(2, 0);
        var end = new Point(0, 2);
        var opt = new LineHelper.BrokenLineOptions { MinimalMargin = 1 };

        var expected = new[]
        {
            new Point(2, 0),
            new Point(2, 1),
            new Point(1, 2),
            new Point(0, 2)
        };
        
        // ACT

        var factPoints = LineHelper.GetBrokenLine(start.X, start.Y, end.X, end.Y, opt);
        
        // ASSERT

        factPoints.Should().BeEquivalentTo(expected);
    }
}