using Core.Combats;
using Core.Dices;

using Stateless;
using System.Text;
using Text.Client;

var clientStateMachine = StateMachineFactory.Create();

var combatCore = new CombatCore(new LinearDice());



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

            HandleOverview(combatCore1: combatCore, stateMachine: clientStateMachine);

            break;
        
        case ClientState.CombatantInfo:
            var combatant = (transition.Parameters[0]) as Combatant;
            
            HandleCombatantInfo(combatCore1: combatCore, stateMachine: clientStateMachine, combatant);

            break;
        
        case ClientState.MoveInfo:
            var movement2 = (CombatMovement)transition.Parameters[0];
            var combatant2 = (transition.Parameters[1]) as Combatant;
            HandleMoveInfo(combatCore1: combatCore, stateMachine: clientStateMachine, combatant2, movement2);
            break;
    }
});

clientStateMachine.Fire(ClientStateTrigger.OnOverview);

void HandleOverview(CombatCore combatCore1, StateMachine<ClientState, ClientStateTrigger> stateMachine)
{
    while (true)
    {
        Console.WriteLine("Round queue:");

        // Print current queue
        var queueSb = new StringBuilder();
        for (var queueIndex = 0; queueIndex < combatCore1.RoundQueue.Count; queueIndex++)
        {
            var combatant = combatCore1.RoundQueue[queueIndex];
            queueSb.Append($" ({combatant.Sid.First()}) {combatant.Sid} |");
        }
        Console.WriteLine(queueSb.ToString());
        Console.WriteLine(new string('=', 10));

        // Print current field

        for (var lineIndex = 0; lineIndex < 3; lineIndex++)
        {
            for (var columnIndex = 1; columnIndex >= 0; columnIndex--)
            {
                var coords = new FieldCoords(columnIndex, lineIndex);
                var heroSlot = combatCore1.Field.HeroSide[coords];
                if (heroSlot.Combatant is not null)
                {
                    Console.Write($" ({heroSlot.Combatant.Sid.First()}) ");
                }
                else
                {
                    Console.Write(" - - ");
                }
            }

            Console.Write("    ");

            for (var columnIndex = 0; columnIndex < 2; columnIndex++)
            {
                var coords = new FieldCoords(columnIndex, lineIndex);
                var monsterSlot = combatCore1.Field.MonsterSide[coords];
                if (monsterSlot.Combatant is not null)
                {
                    Console.Write($" ({monsterSlot.Combatant.Sid.First()}) ");
                }
                else
                {
                    Console.Write(" - - ");
                }
            }

            Console.WriteLine();
        }

        // Print current combatant

        Console.WriteLine($"Turn of {combatCore1.CurrentCombatant.Sid}");

        // Print current combatant moves

        Console.WriteLine("Moves:");
        var mIndex = 0;
        foreach (var movement3 in combatCore1.CurrentCombatant.Hand)
        {
            if (movement3 is not null)
            {
                Console.WriteLine($"{mIndex}: {movement3.Sid}");
            }
            else
            {
                Console.WriteLine($"{mIndex}: -");
            }

            mIndex++;
        }

        Console.WriteLine(new string('=', 10));
        Console.WriteLine("- info {sid} - to display detailed combatant's info");
        Console.WriteLine("- move {movement-index} - to use combat movement");
        Console.WriteLine(new string('=', 10));
        Console.WriteLine("Enter command:");
        var command = Console.ReadLine();

        if (command.StartsWith("info"))
        {
            var combatant = GetCombatantByShortSid(combatCore1, command.Split(" ")[1]);

            stateMachine.Fire(
                new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                    .OnDisplayCombatantInfo),
                combatant);
            break;
        }
        else if (command.StartsWith("move"))
        {
            var split = command.Split(" ");

            var moveNumber = int.Parse(split[1]);

            var selectedMove = combatCore1.CurrentCombatant.Hand.Skip(moveNumber).Take(1).Single();

            var movementExecution = combatCore1.UseCombatMovement(selectedMove);
            PseudoPlayback(movementExecution);
            combatCore1.CompleteTurn();
        }
    }
}

void HandleCombatantInfo(CombatCore combatCore1, StateMachine<ClientState, ClientStateTrigger> stateMachine, Combatant targetCombatant)
{
    Console.WriteLine("Stats:");
    foreach (var stat in targetCombatant.Stats)
    {
        Console.WriteLine($"{stat.Type}: {stat.Value.Current}/{stat.Value.ActualMax}");
    }

    Console.WriteLine("Available movements:");
    var moveIndex = 0;
    foreach (var movement in targetCombatant.Hand)
    {
        if (movement is not null)
        {
            Console.WriteLine($"{moveIndex}: {movement.Sid}");
        }
    }

    while (true)
    {
        Console.WriteLine("Enter command:");
        var command = Console.ReadLine();

        if (command == "info")
        {
            var movement = targetCombatant.Hand.First();
            
            stateMachine.Fire(
                new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                    .OnDisplayMoveInfo),
                movement,
                targetCombatant);
            break;
        }
        else if (command == "overview")
        {
            stateMachine.Fire(ClientStateTrigger.OnOverview);
            break;
        }
    }
}

void HandleMoveInfo(CombatCore combatCore1, StateMachine<ClientState, ClientStateTrigger> stateMachine, Combatant targetCombatant1, CombatMovement movement1)
{
    var selectorContext = combatCore1.GetCurrentSelectorContext();
    Console.WriteLine($"{movement1.Sid} targets:");
    foreach (var effect in movement1.Effects)
    {
        var targets = effect.Selector.Get(targetCombatant1, selectorContext);

        foreach (var combatant in targets)
        {
            Console.WriteLine($"> {combatant.Sid}");
        }
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
                targetCombatant1);
            break;
        }
        else if (command == "overview")
        {
            stateMachine.Fire(ClientStateTrigger.OnOverview);
            break;
        }
    }
}

static void PseudoPlayback(CombatMovementExecution movementExecution)
{
    foreach (var imposeItem in movementExecution.EffectImposeItems)
    {
        foreach (var target in imposeItem.MaterializedTargets)
        {
            imposeItem.ImposeDelegate(target);
        }
    }
}

static Combatant? GetCombatantByShortSid(CombatCore combatCore1, string shortSid)
{
    for (int colIndex = 0; colIndex < 2; colIndex++)
    {
        for (int lineIndex = 0; lineIndex < 3; lineIndex++)
        {
            Combatant? foundCombatant = null;
            foundCombatant = CheckSlot(combatCore1, shortSid, colIndex, lineIndex, combatCore1.Field.HeroSide);

            if (foundCombatant is not null)
            {
                return foundCombatant;
            }

            foundCombatant = CheckSlot(combatCore1, shortSid, colIndex, lineIndex, combatCore1.Field.MonsterSide);
            if (foundCombatant is not null)
            {
                return foundCombatant;
            }
        }
    }
    return null;
}

static Combatant? CheckSlot(CombatCore combatCore1, string shortSid, int colIndex, int lineIndex, CombatFieldSide fieldSide)
{
    var coorods = new FieldCoords(colIndex, lineIndex);
    var testCombatant = fieldSide[coorods].Combatant;
    if (testCombatant is not null)
    {
        var testShortSid = testCombatant.Sid.First();

        if (testShortSid.ToString() == shortSid)
        {
            return testCombatant;
        }
    }

    return null;
}