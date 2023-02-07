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
            var heroSlot = combatCore1.Field.HeroSide[columnIndex, lineIndex];
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
            var monsterSlot = combatCore1.Field.MonsterSide[columnIndex, lineIndex];
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

    while (true)
    {
        Console.WriteLine("Enter command:");
        var command = Console.ReadLine();

        if (command == "info")
        {
            var combatant = combatCore1.Field.HeroSide[0, 1].Combatant;
            stateMachine.Fire(
                new StateMachine<ClientState, ClientStateTrigger>.TriggerWithParameters(ClientStateTrigger
                    .OnDisplayCombatantInfo),
                combatant);
            break;
        }
        else if (command.StartsWith("use"))
        {
            break;
        }
    }
}

void HandleCombatantInfo(CombatCore combatCore1, StateMachine<ClientState, ClientStateTrigger> stateMachine, Combatant targetCombatant)
{
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
    Console.WriteLine($"{movement1.Sid} targets:");
    foreach (var effect in movement1.Effects)
    {
        var targets = effect.Selector.Get(targetCombatant1, combatCore1.Field);

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