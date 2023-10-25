using GameClient.Engine.Ui;

namespace GameClient.Engine.Tests.Ui;

[TestFixture]
public class HoverControlTests
{
    [Test]
    public void HandleHover_WithNonNullValue_RaisesHoverEvent()
    {
        // Arrange
        var hoverController = new HoverController<int>();
        const int TEST_HOVER_VALUE = 5;

        using var monitor = hoverController.Monitor();

        // Act
        hoverController.HandleHover(TEST_HOVER_VALUE);

        // Assert

        monitor.Should().Raise(nameof(hoverController.Hover)).WithArgs<int>(e => e == TEST_HOVER_VALUE);
    }
    
    [Test]
    public void HandleHover_WithNonNullValue_ChangesCurrentValue()
    {
        // Arrange
        var hoverController = new HoverController<int>();
        const int TEST_HOVER_VALUE = 5;

        // Act
        hoverController.HandleHover(TEST_HOVER_VALUE);

        // Assert
        hoverController.CurrentValue.Should().Be(TEST_HOVER_VALUE);
    }

    // [Test]
    // public void HandleHover_WithNullValue_DoesNotChangeCurrentValueAndDoesNotRaiseHoverEvent()
    // {
    //     // Arrange
    //     var hoverController = new HoverController<int>();
    //     int? hoverValue = null;
    //     int? expectedCurrentValue = null;
    //     bool hoverEventRaised = false;
    //     hoverController.Hover += (sender, value) =>
    //     {
    //         hoverEventRaised = true;
    //     };
    //
    //     // Act
    //     hoverController.HandleHover(hoverValue);
    //
    //     // Assert
    //     Assert.AreEqual(expectedCurrentValue, hoverController.CurrentValue);
    //     Assert.IsFalse(hoverEventRaised);
    // }

    [Test]
    public void HandleLeave_WithMatchingValue_ChangesCurrentValueToDefaultAndRaisesLeaveEvent()
    {
        // Arrange
        var hoverController = new HoverController<string>();
        string hoverValue = "hover";
        hoverController.HandleHover(hoverValue);
        string? expectedCurrentValue = null;
        bool leaveEventRaised = false;
        hoverController.Leave += (sender, value) =>
        {
            leaveEventRaised = true;
            Assert.AreEqual(expectedCurrentValue, value);
        };

        // Act
        hoverController.HandleLeave(hoverValue);

        // Assert
        Assert.AreEqual(expectedCurrentValue, hoverController.CurrentValue);
        Assert.IsTrue(leaveEventRaised);
    }

    [Test]
    public void HandleLeave_WithNonMatchingValue_DoesNotChangeCurrentValueAndDoesNotRaiseLeaveEvent()
    {
        // Arrange
        var hoverController = new HoverController<string>();
        string hoverValue = "hover";
        string nonMatchingValue = "nonMatching";
        hoverController.HandleHover(hoverValue);
        string? expectedCurrentValue = hoverValue;
        bool leaveEventRaised = false;
        hoverController.Leave += (sender, value) =>
        {
            leaveEventRaised = true;
        };

        // Act
        hoverController.HandleLeave(nonMatchingValue);

        // Assert
        Assert.AreEqual(expectedCurrentValue, hoverController.CurrentValue);
        Assert.IsFalse(leaveEventRaised);
    }

    [Test]
    public void ForcedDrop_ChangesCurrentValueToDefault()
    {
        // Arrange
        var hoverController = new HoverController<bool>();
        hoverController.HandleHover(true);
        bool? expectedCurrentValue = null;

        // Act
        hoverController.ForcedDrop();

        // Assert
        Assert.AreEqual(expectedCurrentValue, hoverController.CurrentValue);
    }


    [Test]
    public void HandleHover_InitialState_HoverEventRaised()
    {

    }
}