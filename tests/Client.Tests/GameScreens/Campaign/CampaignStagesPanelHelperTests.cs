using Client.GameScreens.Campaign.Ui;

using FluentAssertions;

using NUnit.Framework;

namespace Client.Tests.GameScreens.Campaign;

[TestFixture]
internal class CampaignStagesPanelHelperTests
{
    [Test]
    public void CalcMin_CurrentIsLastMinusOne_ReturnsIndexToLastBeInCenter()
    {
        // ACT

        // 0-1-2-3
        //     ^
        // current
        //..|-----|
        //   window
        var fact = CampaignStagesPanelHelper.CalcMin(current: 2, count: 4, max: 3);

        // ASSERT

        fact.Should().Be(1);
    }

    [Test]
    public void CalcMin_CurrentIsZero_ReturnsZero()
    {
        // ACT

        // 0-1-2-3
        // ^    
        // current
        //|-----|
        //   window
        var fact = CampaignStagesPanelHelper.CalcMin(current: 0, count: 4, max: 3);

        // ASSERT

        fact.Should().Be(0);
    }

    [Test]
    public void CalcMin_CurrentIsInTheMiddle_ReturnsCurrentMinusOne()
    {
        // ACT

        // 0-1-2-3-4
        //     ^
        // current
        //..|-----|
        //   window
        var fact = CampaignStagesPanelHelper.CalcMin(current: 2, count: 5, max: 3);

        // ASSERT

        fact.Should().Be(1);
    }
}