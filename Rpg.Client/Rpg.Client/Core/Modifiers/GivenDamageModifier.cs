namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamageModifier : ModifierBase<float>
    {
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        public float DamageMultiplier { get; set; }

        public override float Modify(float modifiedValue)
        {
            return modifiedValue * DamageMultiplier;
        }
    }
}