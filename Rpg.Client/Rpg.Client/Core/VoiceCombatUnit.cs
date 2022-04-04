using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class VoiceCombatUnit: ICombatUnit
    {
        public VoiceCombatUnit(Unit unit)
        {
            Unit = unit;

            var voiceSkill = new VoiceSkill();
            CombatCards = new[] { new CombatSkill(voiceSkill, new CombatSkillContext(this)) };
        }
        
        public Unit Unit { get; }
        public IReadOnlyList<CombatSkill> CombatCards { get; }
        public int EnergyPool { get; set; }

        public void ChangeState(CombatUnitState targetState)
        {
        }

        public event EventHandler<UnitHitPointsChangedEventArgs>? HasTakenDamage;
        public void RestoreEnergyPoint()
        {
            
        }
    }
}