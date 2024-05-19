using System;

using FluentAssertions;

using NUnit.Framework;

namespace Client.Core.Tests;

[TestFixture]
public class StringHelperTests
{
    [Test]
    public void RichLineBreakingTest()
    {
        // ARRANGE

        var text = "<style=color1>1</style> 2 3";
        var expected = $"<style=color1>1</style> 2{Environment.NewLine}3";

        // ACT

        var fact = StringHelper.RichLineBreaking(text, 3);

        // ASSERT

        fact.Should().Be(expected);
    }

    [Test]
    public void RichLineBreakingTest2()
    {
        // ARRANGE

        var text = "<style=color1>1</style> 2 3";
        var expected = $"<style=color1>1</style>{Environment.NewLine}2{Environment.NewLine}3";

        // ACT

        var fact = StringHelper.RichLineBreaking(text, 1);

        // ASSERT

        fact.Should().Be(expected);
    }
}