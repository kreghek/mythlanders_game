using System.Text;

namespace Rpg.Client.GameScreens.Event.Ui
{
    internal sealed class Speech
    {
        public const float SYMBOL_DELAY_SEC = 0.05f;

        private readonly string _fullText;
        private readonly ISpeechRandomProvider _speechRandomProvider;
        private readonly ISpeechSoundWrapper _speechSound;

        private readonly StringBuilder _textToPrintBuilder;
        private double _delayCounter;
        private int _delayUsed;
        private double _soundDelayCounter;
        private double _symbolDelayCounter;
        private int _symbolIndex;

        public Speech(string fullText, ISpeechSoundWrapper speechSound, ISpeechRandomProvider speechRandomProvider)
        {
            _fullText = fullText;
            FullText = _fullText;
            _speechSound = speechSound;
            _speechRandomProvider = speechRandomProvider;
            _textToPrintBuilder = new StringBuilder(fullText.Length);
        }

        public string FullText { get; }

        public bool IsComplete { get; private set; }

        public string GetCurrentText()
        {
            return _textToPrintBuilder.ToString();
        }

        public void MoveToCompletion()
        {
            IsComplete = true;
            _textToPrintBuilder.Clear();
            _textToPrintBuilder.Append(_fullText);
        }

        public void Update(float elapsedSeconds)
        {
            if (IsComplete)
            {
                return;
            }

            _symbolDelayCounter += elapsedSeconds;

            if (_symbolDelayCounter <= SYMBOL_DELAY_SEC)
            {
                return;
            }

            if (_symbolIndex < _fullText.Length)
            {
                HandleTextSound(elapsedSeconds);

                _textToPrintBuilder.Append(_fullText[_symbolIndex]);
                _symbolDelayCounter = 0;
                _symbolIndex++;
            }
            else
            {
                if (_delayCounter <= 1)
                {
                    _delayCounter += elapsedSeconds;
                }
                else
                {
                    IsComplete = true;
                }
            }
        }

        private void HandleTextSound(float elapsedSeconds)
        {
            if (_soundDelayCounter > 0)
            {
                _soundDelayCounter -= elapsedSeconds;
            }
            else
            {
                if (_delayUsed < 2)
                {
                    var delayRoll = _speechRandomProvider.RollPlayingSoundOnSymbol();
                    if (delayRoll < 0.95)
                    {
                        _speechSound.Play();
                        _delayUsed = 0;
                    }
                    else
                    {
                        _delayUsed++;
                        _soundDelayCounter = _speechSound.Duration / 2;
                    }
                }
                else
                {
                    _speechSound.Play();
                    _delayUsed = 0;
                }
            }
        }
    }
}