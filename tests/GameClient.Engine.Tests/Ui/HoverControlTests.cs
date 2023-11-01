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
    
    [Test]
    public void HandleLeave_HoverThenOldLeave_DoesNotRaiseLeaveEvent()
    {
        // Arrange
        var hoverController = new HoverController<int>();
        const int START_HOVER_VALUE = 5;
        const int TEST_HOVER_VALUE = 6;
        
        using var monitor = hoverController.Monitor();

        // Act
        hoverController.HandleHover(START_HOVER_VALUE);
        hoverController.HandleHover(TEST_HOVER_VALUE);
        hoverController.HandleLeave(START_HOVER_VALUE);

        // Assert
        monitor.Should().NotRaise(nameof(hoverController.Leave));
    }
    
    [Test]
    public void HandleLeave_HoverThenOldLeave_RaisesOnlyHoverEventWithLastValue()
    {
        // Arrange
        var hoverController = new HoverController<int>();
        const int START_HOVER_VALUE = 5;
        const int TEST_HOVER_VALUE = 6;
        
        using var monitor = hoverController.Monitor();

        // Act
        hoverController.HandleHover(START_HOVER_VALUE);
        hoverController.HandleHover(TEST_HOVER_VALUE);
        hoverController.HandleLeave(START_HOVER_VALUE);

        // Assert
        monitor.Should().Raise(nameof(hoverController.Hover)).WithArgs<int>(e => e == TEST_HOVER_VALUE);
    }

    [Test]
    public void HandleLeave_WithMatchingValue_ChangesCurrentValueToDefault()
    {
        // Arrange
        var hoverController = new HoverController<string>();
        const string HOVER_VALUE = "hover";
        hoverController.HandleHover(HOVER_VALUE);

        // Act
        hoverController.HandleLeave(HOVER_VALUE);

        // Assert
        hoverController.CurrentValue.Should().BeNull();
    }
    
    [Test]
    public void HandleLeave_WithMatchingValue_RaisesLeaveEvent()
    {
        // Arrange
        var hoverController = new HoverController<string>();
        const string HOVER_VALUE = "hover";
        hoverController.HandleHover(HOVER_VALUE);

        using var monitor = hoverController.Monitor(); 

        // Act
        hoverController.HandleLeave(HOVER_VALUE);

        // Assert
        monitor.Should().Raise(nameof(hoverController.Leave)).WithArgs<string>(e => e == HOVER_VALUE);
    }
}