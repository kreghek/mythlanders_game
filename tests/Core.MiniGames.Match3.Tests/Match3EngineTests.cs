using Core.Combats;
using Core.MiniGames.Match3.Tests.TestCases;

using Moq;

namespace Core.MiniGames.Match3.Tests;

public class Match3EngineTests
{
    [Test]
    public void Handle_GemsMatchedHorizontally_FieldCleaned()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(3, 1)
        {
            [0, 0] = GemColor.Red,
            [1, 0] = GemColor.Red,
            [2, 0] = GemColor.Red
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());
        
        // ACT
        
        engine.Handle();
        
        // ASSERT

        for (var col = 0; col < engine.Field.Width; col++)
        {
            for (var row = 0; row < engine.Field.Height; row++)
            {
                engine.Field[col, row].Should().Be(GemColor.Empty);
            }
        }
    }
    
    [Test]
    public void Handle_GemsMatchedVertically_FieldCleaned()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(1, 3)
        {
            [0, 0] = GemColor.Red,
            [0, 1] = GemColor.Red,
            [0, 2] = GemColor.Red
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());
        
        // ACT
        
        engine.Handle();
        
        // ASSERT

        for (var col = 0; col < engine.Field.Width; col++)
        {
            for (var row = 0; row < engine.Field.Height; row++)
            {
                engine.Field[col, row].Should().Be(GemColor.Empty);
            }
        }
    }
    
    [Test]
    public void Handle_GemsNotMatched_FieldFilledByGems()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(3, 1)
        {
            [0, 0] = GemColor.Red,
            [1, 0] = GemColor.Green,
            [2, 0] = GemColor.Red
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());
        
        // ACT
        
        engine.Handle();
        
        // ASSERT

        for (var col = 0; col < engine.Field.Width; col++)
        {
            for (var row = 0; row < engine.Field.Height; row++)
            {
                engine.Field[col, row].Should().NotBe(GemColor.Empty);
            }
        }
    }
    
    [Test]
    public void Handle_GemsNotMatched_FieldFilledByGems2()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(3, 3)
        {
            [0, 0] = GemColor.Red,
            [1, 0] = GemColor.Green,
            [2, 0] = GemColor.Red,
            
            [0, 1] = GemColor.Green,
            [1, 1] = GemColor.Red,
            [2, 1] = GemColor.Red,
            
            [0, 2] = GemColor.Red,
            [1, 2] = GemColor.Blue,
            [2, 2] = GemColor.Blue
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());
        
        // ACT
        
        engine.Handle();
        
        // ASSERT

        for (var col = 0; col < engine.Field.Width; col++)
        {
            for (var row = 0; row < engine.Field.Height; row++)
            {
                engine.Field[col, row].Should().NotBe(GemColor.Empty);
            }
        }
    }
    
    [Test]
    public void Handle_GemsMatched_RaiseEvent()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(3, 1)
        {
            [0, 0] = GemColor.Red,
            [1, 0] = GemColor.Red,
            [2, 0] = GemColor.Red
        };

        var expectedCoordsMarched = new[]
        {
            new Coords(0, 0), new Coords(1, 0), new Coords(2, 0)
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());

        var factCoordsMatched = new List<Coords>();
        engine.GemMatched += (_, args) =>
        {
            factCoordsMatched.Add(args.MatchedCoords);
        };

        // ACT
        
        engine.Handle();
        
        // ASSERT

        factCoordsMatched.Should().BeEquivalentTo(expectedCoordsMarched);
    }
    
    [Test]
    public void Handle_EmptyField_NotRaiseEvent()
    {
        // ARRANGE

        var fieldMatrix = new Matrix<GemColor>(3, 1)
        {
            [0, 0] = GemColor.Empty,
            [1, 0] = GemColor.Empty,
            [2, 0] = GemColor.Empty
        };

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());

        using var monitoredEngine = engine.Monitor();
        
        // ACT
        
        engine.Handle();
        
        // ASSERT

        monitoredEngine.Should().NotRaise(nameof(engine.GemMatched));
    }
    
    [Test]
    [TestCaseSource(typeof(Match3EngineTestCases), nameof(Match3EngineTestCases.PrepareField_GemsRainsDown))]
    public void PrepareField_GemsWithEmpty_GemsRainedDown(Matrix<GemColor> fieldMatrix, Matrix<GemColor> expectedMatrix)
    {
        // ARRANGE

        var engine = new Match3Engine(fieldMatrix, Mock.Of<IGemSource>());

      
        // ACT
        
        engine.PrepareField();
        
        // ASSERT

        fieldMatrix.Items.Should().BeEquivalentTo(expectedMatrix.Items);
    }
}