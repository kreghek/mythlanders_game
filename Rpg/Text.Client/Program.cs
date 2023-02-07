using Core.Combats;
using Core.Dices;

using Stateless;

using Text.Client;

var clientStateMachine = new StateMachine<ClientState, ClientStateTrigger>(ClientState.Overview);

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

clientStateMachine.OnTransitionCompleted(transition =>
{
    switch (transition.Destination)
    {
        case ClientState.Overview:
            
            break;
    }
});

var roundIndex = 0;
while (true)
{
    Console.WriteLine($"==== Round {roundIndex} ====");

    Console.WriteLine("Round queue:");
    for (var queueIndex = 0; queueIndex < combatCore.RoundQueue.Count; queueIndex++)
    {
        var combatant = combatCore.RoundQueue[queueIndex];
        Console.WriteLine($"{queueIndex}: {combatant.Sid}");
    }

    while (true)
    {
        Console.WriteLine("Enter command:");
        var command = Console.ReadLine();

        if (command == "list-moves")
        {
            var moveIndex = 0;
            foreach (var movement in combatCore.Field.HeroSide[0, 1].Combatant.Hand)
            {
                if (movement is not null)
                {
                    Console.WriteLine($"{moveIndex}: {movement.Sid}");

                    foreach (var effect in movement.Effects)
                    {
                        Console.WriteLine($"{effect}, {effect.Selector}, {effect.Lifetime}");
                    }
                }
            }
        }

        if (command == "move-info")
        {
            var movement = combatCore.Field.HeroSide[0, 1].Combatant.Hand.First();

            Console.WriteLine($"{movement.Sid}:");
            foreach (var effect in movement.Effects)
            {
                Console.WriteLine($"{effect}, {effect.Selector}, {effect.Lifetime}");
                var targets = effect.Selector.Get(combatCore.Field.HeroSide[0, 1].Combatant, combatCore.Field);

                foreach (var combatant in targets)
                {
                    Console.WriteLine($"{combatant.Sid}");
                }
            }
        }
    }
}