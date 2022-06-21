using System.Linq;

namespace Rpg.Client.Core.SkillEffects
{
    internal sealed class PushBackEffect : InstantaneousEffectBase
    {
        protected override void InfluenceAction()
        {
            if (Target is CombatUnit materializedTarget)
            {
                var targetSlotIndex = materializedTarget.SlotIndex;
                var targetSlotTanking = materializedTarget.IsInTankLine;

                var backCombatUnit = GetBackCombatUnit(targetSlotIndex, materializedTarget.Unit.IsPlayerControlled);
                if (backCombatUnit is not null)
                {
                    materializedTarget.ChangeSlot(backCombatUnit.SlotIndex, backCombatUnit.IsInTankLine);
                    backCombatUnit.ChangeSlot(targetSlotIndex, targetSlotTanking);
                }
            }
        }

        private CombatUnit? GetBackCombatUnit(int targetSlotIndex, bool isPlayerControlled)
        {
            var backSlotIndex = GetBackSlotIndex(targetSlotIndex);
            if (backSlotIndex is null)
            {
                return null;
            }

            var backCombatUnit = CombatContext.Combat.AliveUnits.SingleOrDefault(x =>
                ((CombatUnit)x).SlotIndex == backSlotIndex &&
                ((CombatUnit)x).Unit.IsPlayerControlled == isPlayerControlled);
            if (backCombatUnit is null)
            {
                return null;
            }

            return (CombatUnit)backCombatUnit;
        }

        private static int? GetBackSlotIndex(int targetSlotIndex)
        {
            switch (targetSlotIndex)
            {
                case 0: return 3;
                case 1: return 4;
                case 2: return 5;
                default: return null;
            }
        }
    }
}