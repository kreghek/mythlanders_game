using System;

namespace Rpg.Client.Core.Effects
{
    internal class PeriodicHealEffect : PeriodicEffectBase
    {
        public int Power { get; set; }
        public float PowerMultiplier { get; set; }

        public int ValueRange { get; set; }

        protected override void InfluenceAction()
        {
            var min = Math.Max((int)(Power * PowerMultiplier - ValueRange), 1);
            Target.Unit.TakeHeal(Combat.Dice.Roll(min, (int)(Power * PowerMultiplier + ValueRange)));
            base.InfluenceAction();
        }
    }
}