using Core.Combats;
using Core.Dices;

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
var monster = new Combatant(monsterSequence){Sid = "Digital wolf"};

combatCore.Initialize(
    new[]{new FormationSlot(0, 1){Combatant = hero}},
    new[]{new FormationSlot(1, 1){Combatant = monster}}
    );

var roundIndex = 0;
while (true)
{
    Console.WriteLine($"==== Round {roundIndex} ====");
    
    Console.WriteLine("Heroes:");

    foreach (var formationSlot in combatCore.Field.HeroSide)
    {
        if (formationSlot.Combatant is not null)
        {
            Console.WriteLine($"{formationSlot.ColumnIndex}-{formationSlot.LineIndex}: {formationSlot.Combatant.Sid}");
        }
    }
    
    Console.WriteLine("Monsters:");

    foreach (var formationSlot in combatCore.Field.MonsterSide)
    {
        if (formationSlot.Combatant is not null)
        {
            Console.WriteLine($"{formationSlot.ColumnIndex}-{formationSlot.LineIndex}: {formationSlot.Combatant.Sid}");
        }
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