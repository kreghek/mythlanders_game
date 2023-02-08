using System.Collections.ObjectModel;

using Core.Dices;

namespace Core.Combats;

public class CombatCore
{
    private readonly IList<Combatant> _allUnitList;
    private readonly IList<Combatant> _roundQueue;
    
    private readonly IDice _dice;
    private readonly CombatField _combatField;
    
    private int _roundNumber;

    public IReadOnlyList<Combatant> RoundQueue => _roundQueue.ToArray();

    public CombatCore(IDice dice)
    {
        _dice = dice;
        _combatField = new CombatField();

        _allUnitList = new Collection<Combatant>();
        _roundQueue = new List<Combatant>();
    }

    public void Initialize(IReadOnlyCollection<FormationSlot> heroes, IReadOnlyCollection<FormationSlot> monsters)
    {
        InitializeCombatFieldSide(heroes, _combatField.HeroSide);
        InitializeCombatFieldSide(monsters, _combatField.MonsterSide);

        foreach (var combatant in _allUnitList)
        {
            combatant.StartCombat();
        }

        StartRound();
    }

    public Combatant CurrentCombatant => _roundQueue.FirstOrDefault() ?? throw new InvalidOperationException();

    private void StartRound()
    {
        MakeUnitRoundQueue();

        _roundNumber++;
    }
    
    private void MakeUnitRoundQueue()
    {
        _roundQueue.Clear();

        var orderedByResolve = _allUnitList
            .OrderByDescending(x => x.Stats.Single(s => s.Type == UnitStatType.Resolve).Value.ActualMax)
            .ThenByDescending(x => x.IsPlayerControlled)
            .ToArray();

        foreach (var unit in orderedByResolve)
        {
            if (!unit.IsDead)
            {
                _roundQueue.Add(unit);
            }
        }
    }

    private void InitializeCombatFieldSide(IReadOnlyCollection<FormationSlot> formationSlots, FormationSlot[,] side)
    {
        foreach (var slot in formationSlots)
        {
            if (slot.Combatant is not null)
            {
                side[slot.ColumnIndex, slot.LineIndex].Combatant = slot.Combatant;

                _allUnitList.Add(slot.Combatant);
            }
        }
    }

    public CombatMovementExecution UseCombatMovement(CombatMovement movement)
    {
        var effectContext = new EffectCombatContext(Field, _dice);

        var effectImposeItems = new List<CombatEffectImposeItem>();
        foreach (var effect in movement.Effects)
        {
            void effectImposeDelegate(Combatant materializedTarget)
            {
                effect.Imposer.Impose(effect, materializedTarget, effectContext);
            }

            var effectTargets = effect.Selector.Get(CurrentCombatant, Field);

            var effectImposeItem = new CombatEffectImposeItem(effectImposeDelegate, effectTargets);

            effectImposeItems.Add(effectImposeItem);
        }

        void completeSkillAction()
        {
            //CurrentUnit.EnergyPool -= skill.EnergyCost;

            CompleteStep();
        }

        var movementExecution = new CombatMovementExecution(completeSkillAction, effectImposeItems);

        return movementExecution;
    }

    void CompleteStep()
    {
        throw new NotImplementedException();
    }

    public CombatField Field => _combatField;
}

public delegate void CombatMovementCompleteCallback();

public delegate void CombatEffectImposeDelegate(Combatant target);

public record CombatMovementExecution(
    CombatMovementCompleteCallback CompleteDelegate,
    IReadOnlyCollection<CombatEffectImposeItem> EffectImposeItems);

public record CombatEffectImposeItem(CombatEffectImposeDelegate ImposeDelegate, IReadOnlyList<Combatant> MaterializedTargets);