namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamagePercentageModifier : ModifierBase<float>
    {
        public float Multiplier { get; set; }
        public override ModifierType ModifierType => ModifierType.GivenDamage;

        public override float Modify(float modifiedValue)
        {
            return modifiedValue * (1 + Multiplier);
        }
    }
}