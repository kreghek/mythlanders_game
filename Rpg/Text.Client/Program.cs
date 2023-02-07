using Core.Combats;
using Core.Dices;

var combatCore = new CombatCore(new LinearDice());

var heroSequence = new CombatMovementSequence();
heroSequence.Items.Add(new CombatMovement(){Sid = "Sword slash"});
var hero = new Combatant(heroSequence){ Sid = "Hero"};

var monsterSequence = new CombatMovementSequence();
monsterSequence.Items.Add(new CombatMovement(){ Sid = "Claws" });
var monster = new Combatant(monsterSequence){Sid = "Monster"};

combatCore.Initialize(
    new[]{new FormationSlot(0, 1){Combatant = hero}},
    new[]{new FormationSlot(1, 2){Combatant = monster}}
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
                }
            }
        }
    }
}