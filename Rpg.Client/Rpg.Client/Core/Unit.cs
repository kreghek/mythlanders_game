using System;
using System.Collections.Generic;
using System.Linq;

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

            Stats = new[]
            {
                new UnitStat(UnitStatType.HitPoints, baseValue: GetBaseHitPoint(unitScheme)),
                new UnitStat(UnitStatType.ShieldPoints, baseValue: Armor),
                new UnitStat(UnitStatType.Evasion)
            };

            Skills = new List<ISkill>();
            Perks = new List<IPerk>();

            var equipments = new List<Equipment>();
            InitEquipment(equipments);
            Equipments = equipments;

            ModifyStats();

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

        public bool IsPlayerCombatAvailable => Stats.Single(x => x.Type == UnitStatType.HitPoints).Value.Current > 0;

        public bool IsPlayerControlled { get; init; }

        public int Level { get; private set; }

        public int LevelUpXpAmount => (int)Math.Pow(UnitScheme.UnitBasics.LEVEL_BASE, Level) *
                                      UnitScheme.UnitBasics.LEVEL_MULTIPLICATOR;

        public IList<IPerk> Perks { get; }

        public float Power => CalcPower();

        public IList<ISkill> Skills { get; }

        public IReadOnlyCollection<UnitStat> Stats { get; }

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

            ModifyStats();
        }

        public void RemoveGlobalEffect(GlobalUnitEffect effect)
        {
            _globalEffects.Add(effect);
        }

        public void RestoreHitPointsAfterCombat()
        {
            var HitPoints = Stats.Single(x => x.Type == UnitStatType.HitPoints);
            var hpBonus = (int)Math.Round(HitPoints.Value.ActualMax * UnitScheme.UnitBasics.COMBAT_RESTORE_SHARE,
                MidpointRounding.ToEven);

            HitPoints.Value.Restore(hpBonus);
        }

        public void ChangeScheme(UnitScheme targetUnitScheme)
        {
            var sourceScheme = UnitScheme;
            UnitScheme = targetUnitScheme;
            ModifyStats();

            SchemeAutoTransition?.Invoke(this, new AutoTransitionEventArgs(sourceScheme));
        }

        public event EventHandler<AutoTransitionEventArgs>? SchemeAutoTransition;

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

        private void ApplyStatModifiers(IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> statModifiers)
        {
            foreach (var statModifier in statModifiers)
            {
                var stat = Stats.Single(x => x.Type == statModifier.Item1);
                stat.Value.AddModifier(statModifier.Item2);
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
            ModifyStats();
        }

        private int GetBaseHitPoint(UnitScheme unitScheme)
        {
            var maxHitPoints = unitScheme.HitPointsBase + unitScheme.HitPointsPerLevelBase * (Level - 1);
            var newBase = (int)Math.Round(maxHitPoints, MidpointRounding.AwayFromZero);
            return newBase;
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

        private void ModifyStats()
        {
            ApplyLevels();

            var tempMaxHitPoints = 0f;
            foreach (var perk in Perks)
            {
                var statModifiers = perk.GetStatModifiers();
                if (statModifiers is not null)
                {
                    ApplyStatModifiers(statModifiers);
                }

                perk.ApplyToStats(ref tempMaxHitPoints, ref _armorBonus);
            }

            foreach (var equipment in Equipments)
            {
                var statModifiers = equipment.Scheme.GetStatModifiers(equipment.Level);
                if (statModifiers is not null)
                {
                    ApplyStatModifiers(statModifiers);
                }
            }

            RestoreHp();
        }

        private void RestoreHp()
        {
            foreach (var stat in Stats)
            {
                stat.Value.Restore();
            }
        }

        public event EventHandler<UnitHasBeenDamagedEventArgs>? HasBeenHitPointsDamaged;

        public event EventHandler<UnitHasBeenDamagedEventArgs>? HasBeenShieldPointsDamaged;

        public event EventHandler? HasAvoidedDamage;

        public event EventHandler<int>? HasBeenHitPointsRestored;
        public event EventHandler? Blocked;
        public event EventHandler<int>? HasBeenShieldPointsRestored;

        public event EventHandler<UnitDamagedEventArgs>? Dead;
    }
}