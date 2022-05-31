namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamageModifier : ModifierBase<float>
    {
        public float DamageMultiplier { get; init; }
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        protected override float Modify(float modifiedValue)
        {
            return modifiedValue * DamageMultiplier;
        }
    }
}