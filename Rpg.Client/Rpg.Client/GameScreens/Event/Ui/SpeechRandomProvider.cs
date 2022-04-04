﻿using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Event.Ui
{
    internal sealed class SpeechRandomProvider : ISpeechRandomProvider
    {
        private readonly IDice _dice;

        public SpeechRandomProvider(IDice dice)
        {
            _dice = dice;
        }

        public float RollPlayingSoundOnSymbol()
        {
            return _dice.Roll(100) * 0.01f;
        }
    }
}