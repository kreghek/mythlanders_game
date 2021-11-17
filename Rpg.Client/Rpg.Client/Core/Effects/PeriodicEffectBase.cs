using System.Diagnostics;

namespace Rpg.Client.Core.Effects
{
    internal abstract class PeriodicEffectBase : EffectBase
    {
        private int _value = -1;

        /// <summary>
        /// Duration of effect in combat rounds.
        /// </summary>
        public int Duration
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;
                if (_value == 0)
                {
                    Dispel();
                }
            }
        }

        protected override void InfluenceAction()
        {
            if (Duration < 0)
            {
                Debug.Fail($"{nameof(Duration)} is not defined.");
            }

            Duration--;
        }
    }
}