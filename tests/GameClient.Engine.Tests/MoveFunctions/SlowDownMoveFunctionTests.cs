using Microsoft.Xna.Framework;

namespace GameClient.Engine.MoveFunctions.Tests;

[TestFixture]
public class SlowDownMoveFunctionTests
{
    [Test]
    public void CalcPositionTest()
    {
        // ASSERT

        var start = Vector2.Zero;
        var target = Vector2.UnitX;

        var t = new MoveFunctionValue(0);
        var expectedValue = Vector2.Zero;

        var func = new SlowDownMoveFunction(start, target);

        // ACT

        var factPosition = func.CalcPosition(t);

        // ASSERT

        factPosition.Should().Be(expectedValue);
    }

    [Test]
    public void CalcPositionTest2()
    {
        // ASSERT

        var start = Vector2.Zero;
        var target = Vector2.UnitX;

        var t = new MoveFunctionValue(1);
        var expectedValue = target;

        var func = new SlowDownMoveFunction(start, target);

        // ACT

        var factPosition = func.CalcPosition(t);

        // ASSERT

        factPosition.Should().Be(expectedValue);
    }
}