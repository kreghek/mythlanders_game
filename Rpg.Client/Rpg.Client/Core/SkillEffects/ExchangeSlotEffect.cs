namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class ExchangeSlotEffect : InstantaneousEffectBase
    {
        public CombatUnit Actor { get; set; }

        protected override void InfluenceAction()
        {
            if ( Target is CombatUnit materializedTarget)
            {
                var targetSlotIndex = materializedTarget.SlotIndex;
                var targetSlotTanking = materializedTarget.IsInTankLine;
                materializedTarget.ChangeSlot(Actor.SlotIndex, Actor.IsInTankLine);
                Actor.ChangeSlot(targetSlotIndex, targetSlotTanking);
            }
        }
    }
}
