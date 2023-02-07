using Core.Combats;
using Core.Dices;

using Stateless;

using Text.Client;

var clientStateMachine = new StateMachine<ClientState, ClientStateTrigger>(ClientState.Initialize);

clientStateMachine.Configure(ClientState.Initialize)
    .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

clientStateMachine.Configure(ClientState.Overview)
    .Permit(ClientStateTrigger.OnDisplayCombatantInfo, ClientState.CombatantInfo);

clientStateMachine.Configure(ClientState.CombatantInfo)
    .Permit(ClientStateTrigger.OnDisplayMoveInfo, ClientState.MoveInfo)
    .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

clientStateMachine.Configure(ClientState.MoveInfo)
    .Permit(ClientStateTrigger.OnDisplayCombatantInfo, ClientState.CombatantInfo)
    .Permit(ClientStateTrigger.OnOverview, ClientState.Overview);

var combatCore = new CombatCore(new LinearDice());

var heroSequence = new CombatMovementSequence();
heroSequence.Items.Add(new CombatMovement("Die by sword!", 
    new IEffect[]
    {
        new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectLifetime())
        
    })
);
var hero = new Combatant(heroSequence){ Sid = "Berimir"};

var monsterSequence = new CombatMovementSequence();
monsterSequence.Items.Add(new CombatMovement("Cyber claws", 
    new IEffect[]
    {
        new DamageEffect(new ClosestInLineTargetSelector(), new InstantaneousEffectLifetime())
        
    })
);
var monster = new Combatant(monsterSequence){Sid = "Digital wolf", IsPlayerControlled = false};

combatCore.Initialize(
    new[]{new FormationSlot(0, 1){Combatant = hero}},
    new[]{new FormationSlot(1, 1){Combatant = monster}}
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
    for (var queueIndex = 0; queueIndex < combatCore1.RoundQueue.Count; queueIndex++)
    {
        var combatant = combatCore1.RoundQueue[queueIndex];
        Console.WriteLine($"{queueIndex}: {combatant.Sid}");
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