namespace Rpg.Client.Core.Modifiers
{
    internal class GivenDamagePercentageModifier : ModifierBase<float>
    {
        public override ModifierType ModifierType => ModifierType.GivenDamage;
        public float Multiplier { get; set; }

        protected override float Modify(float modifiedValue)
        {
            return modifiedValue * (1 + Multiplier);
        }
    }
}