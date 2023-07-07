using System;
using System.Collections.Generic;
using System.Linq;

using Core.Combats;

namespace Client.Core.Heroes;

internal sealed class Hero
{
    private float _armorBonus;

    public Hero(UnitScheme unitScheme, int level)
    {
        UnitScheme = unitScheme;
        Level = level;

        Stats = new[]
        {
            new UnitStat(CombatantStatTypes.HitPoints, baseValue: GetBaseHitPoint(unitScheme)),
            new UnitStat(CombatantStatTypes.ShieldPoints, baseValue: Armor),
            new UnitStat(CombatantStatTypes.Resolve, baseValue: unitScheme.Resolve)
        };

        Perks = new List<IPerk>();

        var equipments = new List<Equipment>();
        InitEquipment(equipments);
        Equipments = equipments;

        ModifyStats();
    }

    public int Armor => CalcArmor();

    public IReadOnlyList<Equipment> Equipments { get; }

    public bool IsPlayerControlled { get; init; }

    public int Level { get; private set; }

    public int LevelUpXpAmount => (int)Math.Pow(UnitScheme.UnitBasics.LEVEL_BASE, Level) *
                                  UnitScheme.UnitBasics.LEVEL_MULTIPLICATOR;

    public IList<IPerk> Perks { get; }

    public float Power => CalcPower();

    public IReadOnlyCollection<UnitStat> Stats { get; }

    public int Support => CalcSupport();

    public UnitScheme UnitScheme { get; }

    /// <summary>
    /// Used only by monster units.
    /// Amount of the experience gained for killing this unit.
    /// </summary>
    public int XpReward => Level > 0 ? Level * 20 : (int)(20 * 0.5f);

    public void LevelUp()
    {
        Level++;

        ModifyStats();
    }

    public void RestoreHitPointsAfterCombat()
    {
        var HitPoints = Stats.Single(x => x.Type == CombatantStatTypes.HitPoints);
        var hpBonus = (int)Math.Round(HitPoints.Value.ActualMax * UnitScheme.UnitBasics.COMBAT_RESTORE_SHARE,
            MidpointRounding.ToEven);

        HitPoints.Value.Restore(hpBonus);
    }

    private void ApplyLevels()
    {
        var levels = UnitScheme.Levels;
        if (levels is null)
        {
            return;
        }

        Perks.Clear();

        var levelSchemesToCurrentLevel = levels.OrderBy(x => x.Level).Where(x => x.Level <= Level).ToArray();
        foreach (var levelScheme in levelSchemesToCurrentLevel)
        {
            levelScheme.Apply(this);
        }
    }

    private void ApplyStatModifiers(IReadOnlyCollection<(ICombatantStatType, IUnitStatModifier)> statModifiers)
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
}