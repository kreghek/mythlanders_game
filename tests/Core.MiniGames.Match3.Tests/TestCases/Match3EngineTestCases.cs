using System.Collections;

namespace Core.MiniGames.Match3.Tests.TestCases;

public static class Match3EngineTestCases
{
    // ReSharper disable once InconsistentNaming
    public static IEnumerable PrepareField_GemsRainsDown
    {
        get
        {
            yield return new TestCaseData(
                    new Matrix<GemColor>(1, 2)
                    {
                        [0, 0] = GemColor.Red,

                        // Bottom row is empty
                        [0, 1] = GemColor.Empty
                    },
                    new Matrix<GemColor>(1, 2)
                    {
                        [0, 0] = GemColor.Empty,
                        [0, 1] = GemColor.Red
                    })
            {
                TestName = "The Simplest"   
            };
            
            yield return new TestCaseData(
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Red,
                    [1, 0] = GemColor.Red
                },
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Red,
                    [1, 0] = GemColor.Red
                })
            {
                TestName = "Linear, filled"   
            };
            
            yield return new TestCaseData(
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Red,
                    [1, 0] = GemColor.Empty
                },
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Red,
                    [1, 0] = GemColor.Empty
                })
            {
                TestName = "Linear, semi-filled"   
            };
            
            yield return new TestCaseData(
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Empty,
                    [1, 0] = GemColor.Empty
                },
                new Matrix<GemColor>(2, 1)
                {
                    [0, 0] = GemColor.Empty,
                    [1, 0] = GemColor.Empty
                })
            {
                TestName = "Linear, empty"   
            };
            
            yield return new TestCaseData(
                    new Matrix<GemColor>(1, 3)
                    {
                        [0, 0] = GemColor.Red,
                        [0, 1] = GemColor.Empty, 
                        [0, 2] = GemColor.Red
                    },
                    new Matrix<GemColor>(1, 3)
                    {
                        [0, 0] = GemColor.Empty,
                        [0, 1] = GemColor.Red, 
                        [0, 2] = GemColor.Red
                    })
            {
                TestName = "Empty between two gems"   
            };
            
            yield return new TestCaseData(
                new Matrix<GemColor>(3, 3)
                {
                    [0, 0] = GemColor.Red,
                    [0, 1] = GemColor.Empty, 
                    [0, 2] = GemColor.Empty,
                    
                    [1, 0] = GemColor.Red,
                    [1, 1] = GemColor.Red, 
                    [1, 2] = GemColor.Empty,
                    
                    [2, 0] = GemColor.Empty,
                    [2, 1] = GemColor.Empty, 
                    [2, 2] = GemColor.Red
                },
                new Matrix<GemColor>(3, 3)
                {
                    [0, 0] = GemColor.Empty,
                    [0, 1] = GemColor.Empty, 
                    [0, 2] = GemColor.Red,
                    
                    [1, 0] = GemColor.Empty,
                    [1, 1] = GemColor.Red, 
                    [1, 2] = GemColor.Red,
                    
                    [2, 0] = GemColor.Empty,
                    [2, 1] = GemColor.Empty, 
                    [2, 2] = GemColor.Red
                })
            {
                TestName = "Complex 3x3"   
            };
        }
    }
}