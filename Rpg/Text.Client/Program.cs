﻿using System.Text;

using Core.Combats;
using Core.Combats.CombatantEffects;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Dices;

using Stateless;

namespace Text.Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        var clientStateMachine = StateMachineFactory.Create();

        var combatCore = new CombatCore(new LinearDice());

        combatCore.CombatantHasBeenDamaged += CombatCore_CombatantHasBeenDamaged;

        combatCore.Initialize(
            CombatantFactory.CreateHeroes(),
            CombatantFactory.CreateMonsters()
        );

        var roundIndex = 0;

        clientStateMachine.OnTransitionCompleted(transition =>
        {
            switch (transition.Destination)
            {
                case ClientState.Overview:
                    Console.WriteLine($"==== Round {roundIndex} ====");

                    HandleOverview(combatCore: combatCore, stateMachine: clientStateMachine);

                    break;

                case ClientState.CombatantInfo:
                    var combatant = (Combatant)transition.Parameters[0];

                    HandleCombatantInfo(combatCore, clientStateMachine, combatant);

                    break;

                case ClientState.MoveInfo:
                    var selectedMovement = (CombatMovementInstance)transition.Parameters[0];
                    var combatant2 = (transition.Parameters[1]) as Combatant;
                    HandleMoveDetailedInfo(combatCore: combatCore, stateMachine: clientStateMachine, combatant2,
                        selectedMovement);
                    break;
            }
        });

        clientStateMachine.Fire(ClientStateTrigger.OnOverview);
    }

    private static Combatant? CheckSlot(string shortSid, int colIndex, int lineIndex,
        CombatFieldSide fieldSide)
    {
        var coords = new FieldCoords(colIndex, lineIndex);
        var testCombatant = fieldSide[coords].Combatant;
        if (testCombatant is not null)
        {
            var testShortSid = testCombatant.Sid.First();

            if (testShortSid.ToString() == shortSid) return testCombatant;
        }

        return null;
    }

    private static void CombatCore_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        Console.WriteLine($"! {e.Combatant.Sid} has taken damage {e.Value} to {e.StatType}");
    }

    private static void ExecuteCombatMoveCommand(CombatCore combatCore, string command)
    {
        var split = command.Split(" ");

        var moveNumber = int.Parse(split[1]);

        var selectedMove = combatCore.CurrentCombatant.Hand.Skip(moveNumber).Take(1).Single();

        if (selectedMove is null)
        {
            Console.WriteLine("ERROR: move is null");
            return;
        }

        var movementExecution = combatCore.UseCombatMovement(selectedMove);
        PseudoPlayback(movementExecution);
        combatCore.CompleteTurn();
    }

    private static void ExecuteManeuverCommand(CombatCore combatCore, string command)
    {
        var directionStr = command.Split(" ")[1];

        try
        {
            var direction = directionStr switch
            {
                "up" or "u" => CombatStepDirection.Up,
                "down" or "d" => CombatStepDirection.Down,
                "backward" or "b" => CombatStepDirection.Backward,
                "forward" or "f" => CombatStepDirection.Forward,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (combatCore.CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Current > 0)
                combatCore.UseManeuver(direction);
            else
                Console.WriteLine("ERROR: can't do maneuvers");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("ERROR: can't do maneuvers");
        }
    }

    private static Combatant? GetCombatantByShortSid(CombatCore combatCore1, string shortSid)
    {
        for (var colIndex = 0; colIndex < 2; colIndex++)
            for (var lineIndex = 0; lineIndex < 3; lineIndex++)
            {
                Combatant? foundCombatant = null;
                foundCombatant = CheckSlot(shortSid, colIndex, lineIndex, combatCore1.Field.HeroSide);

                if (foundCombatant is not null) return foundCombatant;

                foundCombatant = CheckSlot(shortSid, colIndex, lineIndex, combatCore1.Field.MonsterSide);
                if (foundCombatant is not null) return foundCombatant;
            }

        return null;
    }

    private static void HandleCombatantInfo(CombatCore combatCore,
        StateMachine<ClientState, ClientStateTrigger> stateMachine, Combatant targetCombatant)
    {
        Console.WriteLine(targetCombatant.Sid);

        Console.WriteLine("Stats:");
        foreach (var stat in targetCombatant.Stats)
            Console.WriteLine($"{stat.Type}: {stat.Value.Current}/{stat.Value.ActualMax}");

        Console.WriteLine("Effects:");
        foreach (var effect in targetCombatant.Effects) PrintCombatantEffectInfo(effect);

        Console.WriteLine("Available movements:");
        PrintMovementsInfo(targetCombatant);

        while (true)
        {
            Console.WriteLine(new string('=', 10));
            Console.WriteLine("- move {movement-index} - to use combat movement");
            Console.WriteLine("- step {direction} - to maneuver. Direction: up/down/forward/backward");
            Console.WriteLine("- overview - to ga to combat overview");
            Console.WriteLine(new string('=', 10));

            Console.WriteLine("Enter command:");
            var command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command)) continue;

            if (command.StartsWith("info"))
            {
                var movement = targetCombatant.Hand
                    .Skip(int.Parse(command.Split(' ')[1]))
                    .Take(1)
                    .Single();

                stateMachine.Fire(
                    new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                        .OnDisplayMoveInfo),
                    movement,
                    targetCombatant);
                break;
            }

            if (command == "overview")
            {
                stateMachine.Fire(ClientStateTrigger.OnOverview);
                break;
            }

            if (command.StartsWith("move"))
            {
                ExecuteCombatMoveCommand(combatCore, command);
                stateMachine.Fire(ClientStateTrigger.OnOverview);
                break;
            }

            if (command.StartsWith("step"))
            {
                ExecuteManeuverCommand(combatCore, command);
                stateMachine.Fire(ClientStateTrigger.OnOverview);
                break;
            }
        }
    }

    private static void HandleMoveDetailedInfo(CombatCore combatCore,
        StateMachine<ClientState, ClientStateTrigger> stateMachine, Combatant targetCombatant,
        CombatMovementInstance movement)
    {
        var selectorContext = combatCore.GetCurrentSelectorContext();
        Console.WriteLine($"{movement.SourceMovement.Sid} targets:");
        foreach (var effect in movement.Effects)
        {
            PrintEffectDetailedInfo(effect);
            var targets = effect.Selector.Get(targetCombatant, selectorContext);

            foreach (var combatant in targets) Console.WriteLine($"> {combatant.Sid}");
        }

        while (true)
        {
            Console.WriteLine("Enter command:");
            var command = Console.ReadLine();

            if (command == "back")
            {
                stateMachine.Fire(
                    new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                        .OnDisplayCombatantInfo),
                    targetCombatant);
                break;
            }

            if (command == "overview")
            {
                stateMachine.Fire(ClientStateTrigger.OnOverview);
                break;
            }
        }
    }

    private static void HandleOverview(CombatCore combatCore,
        StateMachine<ClientState, ClientStateTrigger> stateMachine)
    {
        while (true)
        {
            Console.WriteLine("Round queue:");

            // Print current queue
            var queueSb = new StringBuilder();
            for (var queueIndex = 0; queueIndex < combatCore.RoundQueue.Count; queueIndex++)
            {
                var combatant = combatCore.RoundQueue[queueIndex];
                queueSb.Append(
                    $" ({combatant.Sid.First()}) {combatant.Sid} (R:{combatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.Current}) |");
            }

            Console.WriteLine(queueSb.ToString());
            Console.WriteLine(new string('=', 10));

            // Print current field

            for (var lineIndex = 0; lineIndex < 3; lineIndex++)
            {
                for (var columnIndex = 1; columnIndex >= 0; columnIndex--)
                {
                    var coords = new FieldCoords(columnIndex, lineIndex);
                    var heroSlot = combatCore.Field.HeroSide[coords];
                    if (heroSlot.Combatant is not null)
                        Console.Write($" ({heroSlot.Combatant.Sid.First()}) ");
                    else
                        Console.Write(" - - ");
                }

                Console.Write("    ");

                for (var columnIndex = 0; columnIndex < 2; columnIndex++)
                {
                    var coords = new FieldCoords(columnIndex, lineIndex);
                    var monsterSlot = combatCore.Field.MonsterSide[coords];
                    if (monsterSlot.Combatant is not null)
                        Console.Write($" ({monsterSlot.Combatant.Sid.First()}) ");
                    else
                        Console.Write(" - - ");
                }

                Console.WriteLine();
            }

            // Print current combatant

            Console.Write($"Turn of {combatCore.CurrentCombatant.Sid}");

            if (combatCore.CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Current > 0)
                Console.Write(" (can maneuver)");

            Console.WriteLine();

            // Print current combatant moves

            Console.WriteLine("Moves:");
            PrintMovementsInfo(combatCore.CurrentCombatant);

            Console.WriteLine(new string('=', 10));
            Console.WriteLine("- info {sid} - to display detailed combatant's info");
            Console.WriteLine("- move {movement-index} - to use combat movement");
            Console.WriteLine("- step {direction} - to maneuver. Direction: up/down/forward/backward");
            Console.WriteLine(new string('=', 10));
            Console.WriteLine("Enter command:");
            var command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command)) continue;

            if (command.StartsWith("info"))
            {
                var combatant = GetCombatantByShortSid(combatCore, command.Split(" ")[1]);

                stateMachine.Fire(
                    new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                        .OnDisplayCombatantInfo),
                    combatant);
                break;
            }

            if (command.StartsWith("move"))
                ExecuteCombatMoveCommand(combatCore, command);
            else if (command.StartsWith("step")) ExecuteManeuverCommand(combatCore, command);
        }
    }

    private static void PrintCombatantEffectInfo(ICombatantEffect effect)
    {
        switch (effect)
        {
            case ChangeStateCombatantEffect combatantEffect:
                Console.WriteLine($"{combatantEffect.StatType} +{combatantEffect.Value} on {combatantEffect.Lifetime}");
                break;
        }
    }

    private static void PrintEffectDetailedInfo(IEffectInstance effect)
    {
        Console.WriteLine();
        switch (effect)
        {
            case DamageEffectInstance attackEffect:
                Console.Write($"Attack: {attackEffect.Damage.Min.Current}");
                if (attackEffect.BaseEffect.DamageType != DamageType.Normal) Console.Write($" ({attackEffect.BaseEffect.DamageType})");

                break;

            case ChangeStatEffectInstance buffEffect:
                Console.Write($"Buff: +{buffEffect.BaseEffect.Value} {buffEffect.BaseEffect.TargetStatType} on {buffEffect.BaseEffect.LifetimeType}");
                break;

            case ChangeCurrentStatEffectInstance controlEffect:
                Console.Write($"{controlEffect.BaseEffect.StatValue.Min} {controlEffect.BaseEffect.TargetStatType}");
                break;

            case ChangePositionEffectInstance repositionEffect:
                Console.Write($"{repositionEffect.BaseEffect.Direction}");
                break;
        }

        switch (effect.Selector)
        {
            case ClosestInLineTargetSelector:
                Console.Write(" to the closest in current line");
                break;

            case SelfTargetSelector:
                Console.Write(" to self");
                break;

            case MostShieldChargedTargetSelector:
                Console.Write(" to the most shield changed");
                break;

            case AllVanguardTargetSelector:
                Console.Write(" to all in the vanguard");
                break;
        }

        Console.WriteLine();
    }

    private static void PrintMovementsInfo(Combatant targetCombatant)
    {
        var moveIndex = 0;
        foreach (var movement in targetCombatant.Hand)
        {
            if (movement is not null)
                Console.WriteLine($"{moveIndex}: {movement.SourceMovement.Sid}");
            else
                Console.WriteLine($"{moveIndex}: -");

            moveIndex++;
        }
    }

    private static void PseudoPlayback(CombatMovementExecution movementExecution)
    {
        foreach (var imposeItem in movementExecution.EffectImposeItems)
            foreach (var target in imposeItem.MaterializedTargets)
                imposeItem.ImposeDelegate(target);

        movementExecution.CompleteDelegate();
    }
}