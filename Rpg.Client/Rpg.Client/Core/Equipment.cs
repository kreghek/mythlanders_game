using System;

namespace Rpg.Client.Core
{
    internal sealed class Equipment
    {
        public Equipment(IEquipmentScheme scheme)
        {
            Scheme = scheme;
        }

        public IEquipmentScheme Scheme { get; }
        public int Level { get; private set; }
        public int RequiredResourceAmountToLevelUp => (int)Math.Pow(2, Level);

        public void LevelUp()
        {
            Level++;
            
            GainLevelUp?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? GainLevelUp;
    }
}