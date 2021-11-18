namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamageModifier : ModifierBase<float>
    {
        public float DamageMultiplier { get; init; }
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        public override float Modify(float modifiedValue)
        {
            return modifiedValue * DamageMultiplier;
        }
    }

    internal class GivenDamageAbsoluteModifier : ModifierBase<float>
    {
        public float DamageBonus { get; set; }
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        public override float Modify(float modifiedValue)
        {
            return modifiedValue + DamageBonus;
        }
    }
}