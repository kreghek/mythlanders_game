using System;
using System.Collections;

using NUnit.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.Tests.Core
{
    [TestFixture]
    public class StringHelperTests
    {
        public static IEnumerable GetTestCases()
        {
            var sourceNewLine = StringHelper.SourceNewLineCharacters[0];

            yield return new TestCaseData("1", 60).Returns("1");

            yield return new TestCaseData(new string('1', 10), 10).Returns(new string('1', 10));

            var wordBreaker = StringHelper.WordBreakers[0];

            yield return new TestCaseData(new string('1', 10) + wordBreaker + new string('2', 10), 11)
                .Returns(new string('1', 10) + Environment.NewLine + new string('2', 10));

            yield return new TestCaseData(new string('1', 10)
                                          + wordBreaker + new string('2', 10)
                                          + wordBreaker + new string('3', 10), 11)
                .Returns(new string('1', 10)
                         + Environment.NewLine + new string('2', 10)
                         + Environment.NewLine + new string('3', 10));

            yield return new TestCaseData(new string('1', 10) + "," + wordBreaker + new string('2', 10), 11)
                .Returns(new string('1', 10) + "," + Environment.NewLine + new string('2', 10));

            yield return new TestCaseData(new string('坦', 10) + wordBreaker + new string('塔', 10), 11)
                .Returns(new string('坦', 10) + Environment.NewLine + new string('塔', 10));

            yield return new TestCaseData("1" + wordBreaker + "1", 60).Returns("1 1");

            yield return new TestCaseData("1" + sourceNewLine + "2" + sourceNewLine + "3", 60)
                .Returns("1" + Environment.NewLine + "2" + Environment.NewLine + "3");
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public string LineBreaking_TestCases_ReturnsExpectedWordBreaking(string source, int maxInLine)
        {
            // ACT
            var fact = StringHelper.LineBreaking(source, maxInLine);

            // ASSERT
            return fact;
        }
    }
}