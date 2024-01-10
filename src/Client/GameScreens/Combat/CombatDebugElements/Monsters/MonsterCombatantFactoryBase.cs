using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters;

public abstract class MonsterCombatantFactoryBase: IMonsterCombatantFactory
{
    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories)
    {
        var monsterSequence = CombatMovementSequence(variationIndex);
        
        var stats = CombatantStatsConfig();
        
        var monster = new TestamentCombatant(ClassSid, monsterSequence, stats, combatActorBehaviour,
            combatantStatusFactories)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }

    protected abstract CombatantStatsConfig CombatantStatsConfig();

    protected abstract CombatMovementSequence CombatMovementSequence(int variationIndex);

    protected abstract string ClassSid { get; }
}