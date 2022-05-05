using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class Unit
    {
        private readonly List<GlobalUnitEffect> _globalEffects;

        private float _armorBonus;

        public Unit(UnitScheme unitScheme, int level)
        {
            UnitScheme = unitScheme;
            Level = level;

            HitPoints = new Stat(0);
            Skills = new List<ISkill>();
            Perks = new List<IPerk>();

            var equipments = new List<Equipment>();
            InitEquipment(equipments);
            Equipments = equipments;

            InitBaseStats(unitScheme);

            ShieldPoints = new Stat(Armor);

            _globalEffects = new List<GlobalUnitEffect>();
        }

        public int Armor => CalcArmor();

        public int Damage => CalcDamage();

        public int EnergyPoolSize => UnitScheme.UnitBasics.BASE_MANA_POOL_SIZE +
                                     (Level - 1) * UnitScheme.UnitBasics.MANA_PER_LEVEL;

        public IReadOnlyList<Equipment> Equipments { get; }

        public IReadOnlyCollection<GlobalUnitEffect> GlobalEffects => _globalEffects;

        public bool HasSkillsWithCost
        {
            get
            {
                var manaDependentSkills = Skills.Where(x => x.BaseEnergyCost is not null);
                return manaDependentSkills.Any();
            }
        }

        public Stat HitPoints { get; }

        public bool IsDead => HitPoints.Current <= 0;

        public bool IsPlayerControlled { get; init; }

        public int Level { get; private set; }

        public int LevelUpXpAmount => (int)Math.Pow(UnitScheme.UnitBasics.LEVEL_BASE, Level) *
                                      UnitScheme.UnitBasics.LEVEL_MULTIPLICATOR;

        public IList<IPerk> Perks { get; }

        public float Power => CalcPower();

        public Stat ShieldPoints { get; }

        public IList<ISkill> Skills { get; }

        public int Support => CalcSupport();

        public UnitScheme UnitScheme { get; private set; }

        /// <summary>
        /// Used only by monster units.
        /// Amount of the experience gained for killing this unit.
        /// </summary>
        public int XpReward => Level > 0 ? Level * 20 : (int)(20 * 0.5f);

        public void AddGlobalEffect(IGlobeEvent source)
        {
            var effect = new GlobalUnitEffect(source);
            _globalEffects.Add(effect);
        }

        public void AvoidDamage()
        {
            HasAvoidedDamage?.Invoke(this, EventArgs.Empty);
        }

        public void LevelUp()
        {
            Level++;

            InitBaseStats(UnitScheme);
        }

        public void RemoveGlobalEffect(GlobalUnitEffect effect)
        {
            _globalEffects.Add(effect);
        }

        public void RestoreHitPoints(int heal)
        {
            HitPoints.Restore(heal);
            HasBeenHitPointsRestored?.Invoke(this, heal);
        }

        public void RestoreHitPointsAfterCombat()
        {
            var hpBonus = (int)Math.Round(HitPoints.ActualBase * UnitScheme.UnitBasics.COMBAT_RESTORE_SHARE,
                MidpointRounding.ToEven);

            HitPoints.Restore(hpBonus);
        }

        public void RestoreShields()
        {
            var current = ShieldPoints.Current;
            ShieldPoints.Restore();

            var diff = ShieldPoints.Current - current;

            HasBeenShieldPointsRestored?.Invoke(this, diff);
        }

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
                var autoTransition = UnitScheme.SchemeAutoTransition;
                if (autoTransition is not null)
                {
                    var transformShare = autoTransition.HpShare;
                    var currentHpShare = HitPoints.Share;

                    if (currentHpShare <= transformShare)
                    {
                        var sourceScheme = UnitScheme;
                        UnitScheme = autoTransition.NextScheme;
                        InitBaseStats(UnitScheme);
                        SchemeAutoTransition?.Invoke(this, new AutoTransitionEventArgs(sourceScheme));
                    }
                }
            }

            return new DamageResult
            {
                ValueSource = damageSource,
                ValueFinal = damageToHitPoints
            };
        }

        private int GetAbsorbedDamage(int damage)
        {
            var absorptionPerks = Perks.OfType<Absorption>();
            return (int)Math.Round(damage * absorptionPerks.Count() * 0.1f, MidpointRounding.ToNegativeInfinity);
        }

        private void ApplyLevels()
        {
            var levels = UnitScheme.Levels;
            if (levels is null)
            {
                return;
            }

            Skills.Clear();
            Perks.Clear();

            var levelSchemesToCurrentLevel = levels.OrderBy(x => x.Level).Where(x => x.Level <= Level).ToArray();
            foreach (var levelScheme in levelSchemesToCurrentLevel)
            {
                levelScheme.Apply(this);
            }
        }

        private int CalcArmor()
        {
            var power = Power;
            var powerToArmor = power * UnitScheme.TankRank;
            var armor = UnitScheme.ArmorBase * powerToArmor;

            var armorWithBonus = armor + armor * _armorBonus;

            var normalizedArmor = (int)Math.Round(armorWithBonus, MidpointRounding.AwayFromZero);

            return normalizedArmor;
        }

        private int CalcDamage()
        {
            var power = Power;
            var powerToDamage = power * UnitScheme.DamageDealerRank;
            var damage = UnitScheme.DamageBase * powerToDamage;
            var normalizedDamage = (int)Math.Round(damage, MidpointRounding.AwayFromZero);

            return normalizedDamage;
        }

        private float CalcOverpower()
        {
            //var startPoolSize = ManaPool - (UnitScheme.UnitBasics.BASE_MANA_POOL_SIZE +
            //                                UnitScheme.UnitBasics.MANA_PER_LEVEL *
            //                                UnitScheme.UnitBasics.MINIMAL_LEVEL_WITH_MANA);
            //if (startPoolSize > 0)
            //{
            //    return (float)Math.Log(startPoolSize, UnitScheme.UnitBasics.OVERPOWER_BASE);
            //}

            // Monsters and low-level heroes has no overpower.
            return 0;
        }

        private float CalcPower()
        {
            var powerLevel = CalcPowerLevel();
            var overpower = CalcOverpower();

            return UnitScheme.Power + UnitScheme.PowerPerLevel * powerLevel + overpower;
        }

        private float CalcPowerLevel()
        {
            var powerLevel = Level;

            return powerLevel;
        }

        private int CalcSupport()
        {
            var power = Power;
            var powerToSupport = power * UnitScheme.SupportRank;
            var support = UnitScheme.SupportBase * powerToSupport;
            var normalizedSupport = (int)Math.Round(support, MidpointRounding.AwayFromZero);

            return normalizedSupport;
        }

        private void Equipment_GainLevelUp(object? sender, EventArgs e)
        {
            InitBaseStats(UnitScheme);
        }

        private void InitEquipment(IList<Equipment> equipments)
        {
            if (UnitScheme.Equipments is null)
            {
                return;
            }

            foreach (var equipmentScheme in UnitScheme.Equipments)
            {
                var equipment = new Equipment(equipmentScheme);

                equipment.GainLevelUp += Equipment_GainLevelUp;

                equipments.Add(equipment);
            }
        }

        private void InitBaseStats(UnitScheme unitScheme)
        {
            var maxHitPoints = unitScheme.HitPointsBase + unitScheme.HitPointsPerLevelBase * (Level - 1);

            ApplyLevels();

            foreach (var perk in Perks)
            {
                perk.ApplyToStats(ref maxHitPoints, ref _armorBonus);
            }

            foreach (var equipment in Equipments)
            {
                maxHitPoints *= equipment.Scheme.GetHitPointsMultiplier(equipment.Level);
            }

            var maxHitPointsRounded = (int)Math.Round(maxHitPoints, MidpointRounding.AwayFromZero);
            HitPoints.ChangeBase(maxHitPointsRounded);

            RestoreHp();
        }

        private void RestoreHp()
        {
            HitPoints.Restore();
        }

        private void TakeDamageToHitPoints(int damageSource, int damageAbsorbedByShields)
        {
            HitPoints.Consume(damageAbsorbedByShields);

            var result = new DamageResult
            {
                ValueSource = damageSource, ValueFinal = damageAbsorbedByShields
            };

            var args = new UnitHasBeenDamagedEventArgs
            {
                Result = result
            };
            HasBeenHitPointsDamaged?.Invoke(this, args);
        }

        private void TakeDamageToShields(int damageSource, int damageActual)
        {
            ShieldPoints.Consume(damageActual);

            var result = new DamageResult
            {
                ValueSource = damageSource,
                ValueFinal = damageActual
            };

            HasBeenShieldPointsDamaged?.Invoke(this, new UnitHasBeenDamagedEventArgs { Result = result });
        }

        public event EventHandler<UnitHasBeenDamagedEventArgs>? HasBeenHitPointsDamaged;

        public event EventHandler<UnitHasBeenDamagedEventArgs>? HasBeenShieldPointsDamaged;

        public event EventHandler? HasAvoidedDamage;

        public event EventHandler<int>? HasBeenHitPointsRestored;
        public event EventHandler? Blocked;
        public event EventHandler<int>? HasBeenShieldPointsRestored;

        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public event EventHandler<AutoTransitionEventArgs>? SchemeAutoTransition;
    }
}