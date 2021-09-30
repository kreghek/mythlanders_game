namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamageModifier : ModifierBase<float>
    {
        public float DamageMultiplier { get; set; }
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        public override float Modify(float modifiedValue)
        {
            return modifiedValue * DamageMultiplier;
        }
    }
}