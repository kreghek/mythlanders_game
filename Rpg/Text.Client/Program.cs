using System.Text;

using Core.Combats;
using Core.Combats.BotBehaviour;
using Core.Combats.CombatantEffects;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Dices;

using Stateless;

namespace Text.Client;

internal static class Program
{
    public static void Main()
    {
        var clientStateMachine = StateMachineFactory.Create();

        var combatCore = new CombatCore(new LinearDice());

        combatCore.CombatantHasBeenDamaged += CombatCore_CombatantHasBeenDamaged;
        combatCore.CombatantHasBeenDefeated += CombatCore_CombatantHasBeenDefeated;
        combatCore.CombatantStartsTurn += CombatCore_CombatantStartsTurn;
        combatCore.CombatantEndsTurn += CombatCore_CombatantEndsTurn;

        var manualCombatBehaviour = new ManualCombatActorBehaviour();
        var botCombatBehaviour = new BotCombatActorBehaviour(new IntentionFactory());

        combatCore.Initialize(
            CombatantFactory.CreateHeroes(manualCombatBehaviour),
            CombatantFactory.CreateMonsters(botCombatBehaviour)
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
                    HandleMoveDetailedInfo(combatCore, clientStateMachine, (Combatant)transition.Parameters[1],
                        (CombatMovementInstance)transition.Parameters[0]);
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

    private static void CombatCore_CombatantEndsTurn(object? sender, CombatantEndsTurnEventArgs e)
    {
        Console.WriteLine($"{e.Combatant.Sid} ends turn");
    }

    private static void CombatCore_CombatantHasBeenDamaged(object? sender, CombatantDamagedEventArgs e)
    {
        Console.WriteLine($"! {e.Combatant.Sid} has taken damage {e.Value} to {e.StatType}");
    }

    private static void CombatCore_CombatantHasBeenDefeated(object? sender, CombatantDefeatedEventArgs e)
    {
        Console.WriteLine($"! {e.Combatant.Sid} has been defeated");
    }

    private static void CombatCore_CombatantStartsTurn(object? sender, CombatantTurnStartedEventArgs e)
    {
        Console.WriteLine($"{e.Combatant.Sid} starts turn");
    }

    private static void ExecuteCombatMovementCommand(CombatCore combatCore, string command)
    {
        var split = command.Split(" ");

        var moveNumber = int.Parse(split[1]);

        var selectedMove = combatCore.CurrentCombatant.Hand.Skip(moveNumber).Take(1).Single();

        if (selectedMove is null)
        {
            Console.WriteLine("ERROR: move is null");
            return;
        }

        var movementExecution = combatCore.CreateCombatMovementExecution(selectedMove);
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
                _ => throw new ArgumentOutOfRangeException(nameof(command))
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

    private static Combatant? GetCombatantByShortSid(CombatCore combatCore, string shortSid)
    {
        for (var colIndex = 0; colIndex < 2; colIndex++)
            for (var lineIndex = 0; lineIndex < 3; lineIndex++)
            {
                var foundCombatant = CheckSlot(shortSid, colIndex, lineIndex, combatCore.Field.HeroSide);

                if (foundCombatant is not null) return foundCombatant;

                foundCombatant = CheckSlot(shortSid, colIndex, lineIndex, combatCore.Field.MonsterSide);
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
        PrintMovementsHand(targetCombatant);

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
                ExecuteCombatMovementCommand(combatCore, command);
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
            var targets = effect.Selector.GetMaterialized(targetCombatant, selectorContext);

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
                    PrintSlot(heroSlot);
                }

                Console.Write("    ");

                for (var columnIndex = 0; columnIndex < 2; columnIndex++)
                {
                    var coords = new FieldCoords(columnIndex, lineIndex);
                    var monsterSlot = combatCore.Field.MonsterSide[coords];
                    PrintSlot(monsterSlot);
                }

                Console.WriteLine();
            }

            // Print combatants stats

            for (var columnIndex = 1; columnIndex >= 0; columnIndex--)
                for (var lineIndex = 0; lineIndex < 3; lineIndex++)
                {
                    var coords = new FieldCoords(columnIndex, lineIndex);
                    var heroSlot = combatCore.Field.HeroSide[coords];
                    PrintCombatantShortInfo(heroSlot);
                }

            for (var columnIndex = 0; columnIndex < 2; columnIndex++)
                for (var lineIndex = 0; lineIndex < 3; lineIndex++)
                {
                    var coords = new FieldCoords(columnIndex, lineIndex);
                    var monsterSlot = combatCore.Field.MonsterSide[coords];
                    PrintCombatantShortInfo(monsterSlot);
                }

            // Print current combatant

            Console.WriteLine(new string('-', 5) + "CURRENT" + new string('-', 5));

            Console.Write($"Turn of {combatCore.CurrentCombatant.Sid}");

            if (combatCore.CurrentCombatant.Stats.Single(x => x.Type == UnitStatType.Maneuver).Value.Current > 0)
                Console.Write(" (can maneuver)");

            Console.WriteLine();

            // Print current combatant moves

            Console.WriteLine("Moves:");
            PrintMovementsHand(combatCore.CurrentCombatant);

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
                ExecuteCombatMovementCommand(combatCore, command);
            else if (command.StartsWith("step"))
                ExecuteManeuverCommand(combatCore, command);
            else if (command.StartsWith("wait")) combatCore.Wait();
        }
    }

    private static void PrintCombatantEffectInfo(ICombatantEffect effect)
    {
        switch (effect)
        {
            case ChangeStatCombatantEffect changeStatEffect:
                Console.WriteLine(
                    $"{changeStatEffect.StatType} +{changeStatEffect.Value} during {changeStatEffect.Lifetime}");
                break;

            case ModifyEffectsCombatantEffect modifyEffect:
                Console.WriteLine($"+{modifyEffect.Value} Damage during {modifyEffect.Lifetime}");
                break;
        }
    }

    private static void PrintCombatantShortInfo(FormationSlot slot)
    {
        var combatant = slot.Combatant;
        if (combatant is not null)
            Console.WriteLine(
                $"{combatant.Sid} HP: {combatant.Stats.SingleOrDefault(x => x.Type == UnitStatType.HitPoints).Value.Current} SP: {combatant.Stats.SingleOrDefault(x => x.Type == UnitStatType.ShieldPoints).Value.Current} R: {combatant.Stats.SingleOrDefault(x => x.Type == UnitStatType.Resolve).Value.Current}");
    }

    private static void PrintEffectDetailedInfo(IEffectInstance effect)
    {
        Console.WriteLine();
        switch (effect)
        {
            case DamageEffectInstance attackEffect:
                Console.Write($"Attack: {attackEffect.Damage.Min.ActualMax}");
                if (attackEffect.BaseEffect.DamageType != DamageType.Normal)
                    Console.Write($" ({attackEffect.BaseEffect.DamageType})");

                break;

            case ChangeStatEffectInstance buffEffect:
                Console.Write(
                    $"Buff: +{buffEffect.BaseEffect.Value} {buffEffect.BaseEffect.TargetStatType} on {buffEffect.BaseEffect.LifetimeType}");
                break;

            case ChangeCurrentStatEffectInstance controlEffect:
                Console.Write($"{controlEffect.BaseEffect.StatValue.Min} {controlEffect.BaseEffect.TargetStatType}");
                break;

            case PushToPositionEffectInstance repositionEffect:
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

            case MostShieldChargedEnemyTargetSelector:
                Console.Write(" to the most shield changed");
                break;

            case AllVanguardTargetSelector:
                Console.Write(" to all in the vanguard");
                break;
        }

        Console.WriteLine();
    }

    private static void PrintMovementsHand(Combatant targetCombatant)
    {
        var moveIndex = 0;
        foreach (var movementInstance in targetCombatant.Hand)
        {
            if (movementInstance is not null)
                Console.WriteLine(
                    $"{moveIndex}: {movementInstance.SourceMovement.Sid} (cost: {movementInstance.SourceMovement.Cost.Value})");
            else
                Console.WriteLine($"{moveIndex}: -");

            moveIndex++;
        }
    }

    private static void PrintSlot(FormationSlot slot)
    {
        if (slot.Combatant is not null)
            Console.Write($" ({slot.Combatant.Sid.First()}) ");
        else
            Console.Write(" - - ");
    }

    private static void PseudoPlayback(CombatMovementExecution movementExecution)
    {
        foreach (var imposeItem in movementExecution.EffectImposeItems)
            foreach (var target in imposeItem.MaterializedTargets)
                imposeItem.ImposeDelegate(target);

        movementExecution.CompleteDelegate();
    }
}