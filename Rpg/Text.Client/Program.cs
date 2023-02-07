using Core.Combats;
using Core.Dices;

var combatCore = new CombatCore(new LinearDice());

var hero = new Combatant(){ Sid = "Hero"};

var monster = new Combatant(){Sid = "Monster"};

combatCore.Initialize(
    new[]{new FormationSlot(1){Combatant = hero}},
    new[]{new FormationSlot(2){Combatant = monster}}
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
            Console.WriteLine($"{formationSlot.Index}: {formationSlot.Combatant.Sid}");
        }
    }
    
    Console.WriteLine("Monsters:");

    foreach (var formationSlot in combatCore.Field.MonsterSide)
    {
        if (formationSlot.Combatant is not null)
        {
            Console.WriteLine($"{formationSlot.Index}: {formationSlot.Combatant.Sid}");
        }
    }

    Console.ReadLine();
}