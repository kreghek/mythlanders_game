using System;
using System.Collections;
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

        private float _armorBonus;

        public Unit(UnitScheme unitScheme, int level) : this(unitScheme, level,
            equipmentLevel: 0,
            xp: 0,
            equipmentItems: 0)
        {
        }

        public Unit(UnitScheme unitScheme, int level, int equipmentLevel, int xp, int equipmentItems)
        {
            UnitScheme = unitScheme;

            Perks = unitScheme.Perks;

            Level = level;
            Xp = xp;
            EquipmentLevel = equipmentLevel;
            EquipmentItems = equipmentItems;

            InitStats(unitScheme);
            RestoreHp();

            ManaPool = 0;
        }

        public int Armor => CalcArmor();

        public int Damage => CalcDamage();

        public int EquipmentItems { get; private set; }

        public int EquipmentLevel { get; set; }

        public int EquipmentLevelup => (int)Math.Pow(2, EquipmentLevel);

        public int EquipmentRemains => EquipmentLevelup - EquipmentItems;

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

        public int Level { get; set; }
        public int LevelUpXp => (int)Math.Pow(LEVEL_BASE, Level) * LEVEL_MULTIPLICATOR;

        public int ManaPool { get; set; }
        public int ManaPoolSize => BASE_MANA_POOL_SIZE + (Level - 1) * MANA_PER_LEVEL;

        public int MaxHitPoints { get; set; }

        public IEnumerable<IPerk> Perks { get; }

        public float Power => CalcPower();

        public IReadOnlyList<ISkill> Skills { get; set; }

        public int SkillSetIndex
        {
            get
            {
                var skillSetIndex = 0;
                if (EquipmentLevel > 0)
                {
                    skillSetIndex = EquipmentLevel - 1;
                }

                var skillSetIndexNormalized = Math.Min(skillSetIndex, UnitScheme.SkillSets.Count - 1);

                return skillSetIndexNormalized;
            }
        }

        public int Support => CalcSupport();

        public UnitScheme UnitScheme { get; private set; }

        public int Xp { get; set; }

        public int XpRemains => LevelUpXp - Xp;

        /// <summary>
        /// Used only by monster units.
        /// Amount of the experience gained for killing this unit.
        /// </summary>
        public int XpReward => Level * 20;

        public void AvoidDamage()
        {
            HasAvoidedDamage?.Invoke(this, EventArgs.Empty);
        }

        public bool GainEquipmentItem(int amount)
        {
            var items = EquipmentItems;
            var level = EquipmentLevel;
            var equipmentLevelup = EquipmentLevelup;
            var equipmentRemains = EquipmentRemains;
            var wasLevelUp = GainCounterInner(amount, ref items, ref level, ref equipmentLevelup, ref equipmentRemains);
            EquipmentItems = items;
            EquipmentLevel = level;

            if (wasLevelUp)
            {
                InitStats(UnitScheme);
            }

            return wasLevelUp;
        }

        /// <summary>
        /// Increase XP.
        /// </summary>
        /// <returns>Returns true is level up.</returns>
        public bool GainXp(int amount)
        {
            var xp = Xp;
            var level = Level;
            var levelupXp = LevelUpXp;
            var xpRemains = XpRemains;
            var wasLevelUp = GainCounterInner(amount, ref xp, ref level, ref levelupXp, ref xpRemains);
            Xp = xp;
            Level = level;

            if (wasLevelUp)
            {
                InitStats(UnitScheme);
            }

            return wasLevelUp;
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

        public DamageResult TakeDamage(CombatUnit damageDealer, int damageSource)
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
                    var share = autoTransition.HpShare;
                    var currentHpShare = (float)HitPoints / MaxHitPoints;

                    if (share <= currentHpShare)
                    {
                        UnitScheme = autoTransition.NextScheme;
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
            float powerLevel;
            if (EquipmentLevel > 0)
            {
                powerLevel = (Level * 0.5f + EquipmentLevel * 0.5f);
            }
            else
            {
                // The monsters do not use equipment level. They has no equipment at all.
                powerLevel = Level;
            }

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

        private static bool GainCounterInner(int amount, ref int xp, ref int level, ref int levelupXp,
            ref int xpRemains)
        {
            var currentXpCounter = amount;
            var wasLevelup = false;

            while (currentXpCounter > 0)
            {
                var xpToNextLevel = Math.Min(currentXpCounter, xpRemains);
                currentXpCounter -= xpToNextLevel;

                xp += xpToNextLevel;

                if (xp >= levelupXp)
                {
                    level++;
                    xp = 0;

                    wasLevelup = true;
                }
            }

            return wasLevelup;
        }

        private void InitStats(UnitScheme unitScheme)
        {
            var maxHitPoints = (int)Math.Round(
                unitScheme.HitPointsBase + unitScheme.HitPointsPerLevelBase * Level,
                MidpointRounding.AwayFromZero);

            foreach (var perk in Perks)
            {
                perk.ApplyToStats(ref maxHitPoints, ref _armorBonus);
            }

            MaxHitPoints = maxHitPoints;

            Skills = unitScheme.SkillSets[SkillSetIndex].Skills;
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
}