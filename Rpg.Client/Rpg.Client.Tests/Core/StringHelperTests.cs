using System;
using System.Collections;

using NUnit.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Tests.Core
{
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public string LineBreaking_TestCases_ReturnsExpectedWordBreaking(string source, int maxInLine)
        {
            // ACT
            var fact = StringHelper.LineBreaking(source, maxInLine);
            
            // ASSERT
            return fact;
        }

        public static IEnumerable GetTestCases()
        {
            yield return new TestCaseData("1", 60).Returns("1");
            
            yield return new TestCaseData(new string('1', 10), 10).Returns(new string('1', 10));
            
            yield return new TestCaseData(new string('1', 10) + " " + new string('2', 10), 11)
                .Returns(new string('1', 10) + Environment.NewLine + new string('2', 10));
            
            yield return new TestCaseData(new string('1', 10) + ", " + new string('2', 10), 11)
                .Returns(new string('1', 10) + "," + Environment.NewLine + new string('2', 10));
            
            yield return new TestCaseData(new string('坦', 10) + " " + new string('塔', 10), 11)
                .Returns(new string('坦', 10) + Environment.NewLine + new string('塔', 10));
        }
    }
}