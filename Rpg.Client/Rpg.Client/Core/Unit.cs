using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal sealed class Unit
    {
        private const int BASE_MANA_POOL_SIZE = 3;
        private const int MANA_PER_LEVEL = 1;
        private const float COMBAT_RESTORE_SHARE = 1.0f;

        private const int LEVEL_BASE = 2;
        private const int LEVEL_MULTIPLICATOR = 100;

        private const float OVERPOWER_BASE = 2;
        private const int MINIMAL_LEVEL_WITH_MANA = 2;
        private readonly List<GlobalUnitEffect> _globalEffects;

        private float _armorBonus;

        public Unit(UnitScheme unitScheme, int level)
        {
            UnitScheme = unitScheme;

            Skills = new List<ISkill>();
            Perks = new List<IPerk>();
            var equipments = new List<Equipment>();
            InitEquipment(equipments);
            Equipments = equipments;

            Level = level;

            InitStats(unitScheme);

            ManaPool = 0;

            _globalEffects = new List<GlobalUnitEffect>();
        }

        public int Armor => CalcArmor();

        public int Damage => CalcDamage();

        public IReadOnlyList<Equipment> Equipments { get; }

        public IReadOnlyCollection<GlobalUnitEffect> GlobalEffects => _globalEffects;

        public bool HasSkillsWithCost
        {
            get
            {
                var manaDependentSkills = Skills.Where(x => x.ManaCost is not null);
                return manaDependentSkills.Any();
            }
        }

        public int HitPoints { get; set; }

        public bool IsDead => HitPoints <= 0;

        public bool IsPlayerControlled { get; init; }

        public int Level { get; private set; }
        public int LevelUpXpAmount => (int)Math.Pow(LEVEL_BASE, Level) * LEVEL_MULTIPLICATOR;

        public int ManaPool { get; set; }
        public int ManaPoolSize => BASE_MANA_POOL_SIZE + (Level - 1) * MANA_PER_LEVEL;

        public int MaxHitPoints { get; private set; }

        public IList<IPerk> Perks { get; }

        public float Power => CalcPower();

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

        public float GetEquipmentAttackMultiplier(SkillSid skillSid)
        {
            var m = 1f;

            foreach (var equipment in Equipments)
            {
                m *= equipment.Scheme.GetDamageMultiplier(skillSid, equipment.Level);
            }

            return m;
        }

        public void LevelUp()
        {
            Level++;

            InitStats(UnitScheme);
        }

        public void RemoveGlobalEffect(GlobalUnitEffect effect)
        {
            _globalEffects.Add(effect);
        }

        public void RestoreHitPoints(int heal)
        {
            HitPoints += Math.Min(MaxHitPoints - HitPoints, heal);
            HasBeenHealed?.Invoke(this, heal);
        }

        public void RestoreHitPointsAfterCombat()
        {
            var hpBonus = (int)Math.Round(MaxHitPoints * COMBAT_RESTORE_SHARE, MidpointRounding.ToEven);

            HitPoints += hpBonus;

            if (HitPoints > MaxHitPoints)
            {
                HitPoints = MaxHitPoints;
            }
        }

        public DamageResult TakeDamage(ICombatUnit damageDealer, int damageSource)
        {
            var damageAbsorbedByArmor = Math.Max(damageSource - Armor, 0);
            HitPoints -= Math.Min(HitPoints, damageAbsorbedByArmor);

            var result = new DamageResult
            {
                ValueSource = damageSource,
                ValueFinal = damageAbsorbedByArmor
            };

            var args = new UnitHasBeenDamagedEventArgs { Result = result };
            HasBeenDamaged?.Invoke(this, args);

            if (HitPoints <= 0)
            {
                Dead?.Invoke(this, new UnitDamagedEventArgs(damageDealer));
            }
            else
            {
                var autoTransition = UnitScheme.SchemeAutoTransition;
                if (autoTransition is not null)
                {
                    var transformShare = autoTransition.HpShare;
                    var currentHpShare = (float)HitPoints / MaxHitPoints;

                    if (currentHpShare <= transformShare)
                    {
                        UnitScheme = autoTransition.NextScheme;
                        InitStats(UnitScheme);
                        SchemeAutoTransition?.Invoke(this, new AutoTransitionEventArgs());
                    }
                }
            }

            return result;
        }

        internal void RestoreManaPoint()
        {
            if (ManaPool < ManaPoolSize)
            {
                ManaPool++;
            }
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
            var startPoolSize = ManaPool - (BASE_MANA_POOL_SIZE + MANA_PER_LEVEL * MINIMAL_LEVEL_WITH_MANA);
            if (startPoolSize > 0)
            {
                return (float)Math.Log(startPoolSize, OVERPOWER_BASE);
            }

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
            InitStats(UnitScheme);
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

        private void InitStats(UnitScheme unitScheme)
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

            MaxHitPoints = (int)Math.Round(maxHitPoints, MidpointRounding.AwayFromZero);

            RestoreHp();
        }

        private void RestoreHp()
        {
            HitPoints = MaxHitPoints;
        }

        public event EventHandler<UnitHasBeenDamagedEventArgs>? HasBeenDamaged;

        public event EventHandler? HasAvoidedDamage;

        public event EventHandler<int>? HasBeenHealed;

        public event EventHandler<UnitDamagedEventArgs>? Dead;

        public event EventHandler<AutoTransitionEventArgs>? SchemeAutoTransition;
    }

    internal sealed class GlobalUnitEffect
    {
        public GlobalUnitEffect(IGlobeEvent source)
        {
            Source = source;
        }

        public IGlobeEvent Source { get; }
    }
}