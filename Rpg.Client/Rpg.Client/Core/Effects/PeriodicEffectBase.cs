using System.Diagnostics;

namespace Rpg.Client.Core.Effects
{
    internal abstract class PeriodicEffectBase : EffectBase
    {
        private int _value = -1;

        public int Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;
                if (_value == 0)
                    Dispel();
            }
        }

        protected override void InfluenceAction()
        {
            if (Value < 0)
                Debug.Fail("Value не задано");

            Value--;
        }
    }
}