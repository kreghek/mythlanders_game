using System;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.GameScreens.Event.Ui;

namespace Rpg.Client.Tests.GameScreens.Event.Ui
{
    [TestFixture]
    public class SpeechTests
    {
        [Test]
        public void Update_SingleSymbol_CurrentTextIsSymbol()
        {
            // ARRANGE

            const string FULL_TEXT = "1";
            
            var speechRandomProvider = Mock.Of<ISpeechRandomProvider>();

            var speech = new Speech(FULL_TEXT, Mock.Of<ISpeechSoundWrapper>(), speechRandomProvider);
            
            // ACT
            
            speech.Update(Speech.SYMBOL_DELAY_SEC * 2);
            
            // ASSERT

            speech.GetCurrentText().Should().Be(FULL_TEXT);
        }
        
        [Test]
        public void Update_UpdateAfterComplete_DoesNotThrowException()
        {
            // ARRANGE

            const string FULL_TEXT = "1";
            
            var speechRandomProvider = Mock.Of<ISpeechRandomProvider>();

            var speech = new Speech(FULL_TEXT, Mock.Of<ISpeechSoundWrapper>(), speechRandomProvider);
            
            // ACT

            Action act = () =>
            {
                for (var iteration = 0; iteration < FULL_TEXT.Length; iteration++)
                {
                    speech.Update(Speech.SYMBOL_DELAY_SEC * 2);
                }  
            };

            // ASSERT

            act.Should().NotThrow();
        }
    }
}