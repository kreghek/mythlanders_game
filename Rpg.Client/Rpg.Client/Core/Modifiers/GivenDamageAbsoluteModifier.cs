namespace Rpg.Client.Core.Modifiers
{
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