﻿namespace Rpg.Client.Core.Modifiers
{
    internal class TakenDamageModifier : ModifierBase<float>
    {
        public float DamageMultiplier { get; set; }
        public override ModifierType ModifierType => ModifierType.TakenDamage;

        protected override float Modify(float modifiedValue)
        {
            return modifiedValue * DamageMultiplier;
        }
    }
}