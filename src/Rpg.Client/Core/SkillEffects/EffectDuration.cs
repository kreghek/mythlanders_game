namespace Rpg.Client.Core.SkillEffects
{
    internal struct EffectDuration
    {
        private int _value;
        private readonly bool _compensation;
        private bool _compensationUsed;

        public EffectDuration(int value, bool compensation = false)
        {
            _value = value;
            if (compensation)
            {
                _value++;
            }

            _compensation = compensation;
            _compensationUsed = false;
        }

        public void Decrease()
        {
            _value--;
            _compensationUsed = true;
        }

        public void Increase(EffectDuration other)
        {
            _value += other._value;
        }

        public override string ToString()
        {
            var decompensationValue = _value;
            if (_compensation && _compensationUsed)
            {
                decompensationValue = _value - 1;
            }

            return decompensationValue.ToString();
        }

        public bool IsOut => _value <= 0;
    }
}