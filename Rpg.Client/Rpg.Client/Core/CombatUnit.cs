using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class CombatUnit : ICombatUnit
    {
        public CombatUnit(Unit unit, GroupSlot slot)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            CurrentUnitScheme = unit.UnitScheme;
            SlotIndex = slot.Index;
            IsInTankLine = slot.IsTankLine;
            EnergyPool = unit.EnergyPoolSize;

            Stats = unit.Stats.Select(x => new CombatUnitStat(x)).ToArray();

            var skillContext = new CombatSkillContext(this);
            CombatCards = CreateCombatSkills(unit.Skills, skillContext);

            unit.HasBeenHitPointsDamaged += Unit_HasBeenHitPointsDamaged;
            unit.HasBeenShieldPointsDamaged += Unit_HasBeenShieldPointsDamaged;
            unit.HasBeenHitPointsRestored += Unit_BeenHealed;
            unit.HasBeenShieldPointsRestored += Unit_HasBeenShieldRestored;
            unit.HasAvoidedDamage += Unit_HasAvoidedDamage;
            unit.Blocked += Unit_Blocked;

            unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        }

        public void RestoreHitPoints(int heal)
        {
            HitPoints.Restore(heal);
            var args = new UnitStatChangedEventArgs { CombatUnit = this, Amount = heal };
            HasBeenHitPointsRestored?.Invoke(this, args);
        }

        public void RestoreShields()
        {
            var current = ShieldPoints.Current;
            ShieldPoints.Restore();

            var diff = ShieldPoints.Current - current;
            var args = new UnitStatChangedEventArgs { CombatUnit = this, Amount = diff };

            HasBeenShieldPointsRestored?.Invoke(this, args);
        }
        private int GetAbsorbedDamage(int damage)
        {
            var absorptionPerks = Unit.Perks.OfType<Absorption>();
            return (int)Math.Round(damage * absorptionPerks.Count() * 0.1f, MidpointRounding.ToNegativeInfinity);
        }

        private void TakeDamageToHitPoints(int damageSource, int damageAbsorbedByShields)
        {
            HitPoints.Consume(damageAbsorbedByShields);

            var result = new DamageResult
            {
                ValueSource = damageSource, ValueFinal = damageAbsorbedByShields
            };

            var args = new UnitStatChangedEventArgs
            {
                CombatUnit = this,
                Amount = result.ValueFinal.Value,
                SourceAmount = result.ValueSource,
                Direction = HitPointsChangeDirection.Negative
            };

            HasTakenHitPointsDamage?.Invoke(this, args);
        }

        private void TakeDamageToShields(int damageSource, int damageActual)
        {
            ShieldPoints.Consume(damageActual);

            var result = new DamageResult
            {
                ValueSource = damageSource,
                ValueFinal = damageActual
            };

            var args = new UnitStatChangedEventArgs
            {
                CombatUnit = this,
                Amount = result.ValueFinal.Value,
                SourceAmount = result.ValueSource,
                Direction = HitPointsChangeDirection.Negative
            };

            HasTakenShieldPointsDamage?.Invoke(this, args);
        }

        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public DamageResult TakeDamage(ICombatUnit damageDealer, int damageSource)
        {
            var absorbtion = GetAbsorbedDamage(damageSource);

            var damageAbsorbedByPerks = Math.Max(damageSource - absorbtion, 0);

            var damageToShield = Math.Min(ShieldPoints.Current, damageAbsorbedByPerks);
            var damageToHitPoints = damageAbsorbedByPerks - damageToShield;

            var blocked = true;
            if (damageToShield > 0)
            {
                TakeDamageToShields(damageSource, damageToShield);
                blocked = false;
            }

            if (damageToHitPoints > 0)
            {
                TakeDamageToHitPoints(damageSource, damageToHitPoints);
                blocked = false;
            }

            if (blocked)
            {
                Blocked?.Invoke(this, EventArgs.Empty);
            }

            if (HitPoints.Current <= 0)
            {
                Dead?.Invoke(this, new UnitDamagedEventArgs(damageDealer));
            }
            else
            {
                var autoTransition = CurrentUnitScheme.SchemeAutoTransition;
                if (autoTransition is not null)
                {
                    var transformShare = autoTransition.HpShare;
                    var currentHpShare = HitPoints.GetShare();

                    if (currentHpShare <= transformShare)
                    {
                        Unit.ChangeScheme(autoTransition.NextScheme);
                    }
                }
            }

            return new DamageResult
            {
                ValueSource = damageSource,
                ValueFinal = damageToHitPoints
            };
        }

        public UnitScheme CurrentUnitScheme { get; private set; }

        public bool IsDead => HitPoints.Current <= 0;
        public IStatValue ShieldPoints => Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value;
        public IStatValue HitPoints => Stats.Single(x => x.Type == UnitStatType.HitPoints).Value;

        public bool IsInTankLine { get; private set; }

        public bool IsWaiting { get; set; }

        public int SlotIndex { get; private set; }

        public CombatUnitState State { get; private set; }

        public CombatSkill? TargetSkill { get; set; }

        public TargetSlot? TargetSlot { get; set; }

        public void UnsubscribeHandlers()
        {
            Unit.HasBeenHitPointsDamaged -= Unit_HasBeenHitPointsDamaged;
            Unit.HasBeenShieldPointsDamaged -= Unit_HasBeenShieldPointsDamaged;
            Unit.HasBeenHitPointsRestored -= Unit_BeenHealed;
            Unit.HasBeenShieldPointsRestored -= Unit_HasBeenShieldRestored;
            Unit.HasAvoidedDamage -= Unit_HasAvoidedDamage;
            Unit.SchemeAutoTransition -= Unit_SchemeAutoTransition;
            Unit.Blocked -= Unit_Blocked;
        }

        internal void ChangeSlot(int slotIndex, bool isInTankLine)
        {
            SlotIndex = slotIndex;
            IsInTankLine = isInTankLine;
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        private static IReadOnlyList<CombatSkill> CreateCombatSkills(IEnumerable<ISkill> unitSkills,
            ICombatSkillContext combatSkillContext)
        {
            return unitSkills.Select(skill => new CombatSkill(skill, combatSkillContext)).ToList();
        }

        private void Unit_BeenHealed(object? sender, int e)
        {
            HasBeenHitPointsRestored?.Invoke(this, new UnitStatChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_Blocked(object? sender, EventArgs e)
        {
            Blocked?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_HasAvoidedDamage(object? sender, EventArgs e)
        {
            HasAvoidedDamage?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_HasBeenHitPointsDamaged(object? sender, UnitHasBeenDamagedEventArgs e)
        {
            Debug.Assert(e.Result is not null);
            Debug.Assert(e.Result?.ValueFinal is not null);

            var args = new UnitStatChangedEventArgs
            {
                CombatUnit = this,
                Amount = e.Result.ValueFinal.Value,
                SourceAmount = e.Result.ValueSource,
                Direction = HitPointsChangeDirection.Negative
            };
            HasTakenHitPointsDamage?.Invoke(this, args);
        }

        private void Unit_HasBeenShieldPointsDamaged(object? sender, UnitHasBeenDamagedEventArgs e)
        {
            Debug.Assert(e.Result is not null);
            Debug.Assert(e.Result?.ValueFinal is not null);
            var args = new UnitStatChangedEventArgs
            {
                CombatUnit = this,
                Amount = e.Result.ValueFinal.Value,
                SourceAmount = e.Result.ValueSource,
                Direction = HitPointsChangeDirection.Negative
            };
            HasTakenShieldPointsDamage?.Invoke(this, args);
        }

        private void Unit_HasBeenShieldRestored(object? sender, int e)
        {
            HasBeenShieldPointsRestored?.Invoke(this,
                new UnitStatChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
        {
            TargetSlot = null;
            TargetSkill = null;

            var skillContext = new CombatSkillContext(this);
            CombatCards = CreateCombatSkills(Unit.Skills, skillContext);
        }

        public IReadOnlyList<CombatSkill> CombatCards { get; private set; }

        public int EnergyPool { get; set; }

        public void ChangeState(CombatUnitState targetState)
        {
            State = targetState;
        }

        public Unit Unit { get; }

        public IReadOnlyCollection<IUnitStat> Stats { get; }

        public event EventHandler<UnitStatChangedEventArgs>? HasTakenHitPointsDamage;

        public void RestoreEnergyPoint()
        {
            EnergyPool++;
            if (EnergyPool > Unit.EnergyPoolSize)
            {
                EnergyPool = Unit.EnergyPoolSize;
            }
        }

        public event EventHandler<UnitStatChangedEventArgs>? HasTakenShieldPointsDamage;

        public event EventHandler? Blocked;

        internal event EventHandler<UnitStatChangedEventArgs>? HasBeenHitPointsRestored;

        internal event EventHandler<UnitStatChangedEventArgs>? HasBeenShieldPointsRestored;

        internal event EventHandler? HasAvoidedDamage;

        internal event EventHandler? PositionChanged;
    }
}