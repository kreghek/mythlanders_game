using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class VoiceCombatUnit : ICombatUnit
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

        public IReadOnlyCollection<IUnitStat> Stats => throw new NotImplementedException();

        public bool IsDead => throw new NotImplementedException();

        public void ChangeState(CombatUnitState targetState)
        {
        }

        public event EventHandler<UnitStatChangedEventArgs>? HasTakenHitPointsDamage;
        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public void RestoreEnergyPoint()
        {
        }

        public void RestoreHitPoints(int heal)
        {
            throw new NotImplementedException();
        }

        public void RestoreShields()
        {
            throw new NotImplementedException();
        }

        public DamageResult TakeDamage(ICombatUnit damageDealer, int damageSource)
        {
            throw new NotImplementedException();
        }
    }
}