using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters;

public abstract class MonsterCombatantFactoryBase : IMonsterCombatantFactory
{
    protected virtual string ClassSid => GetType().Name.Substring(0, GetType().Name.Length - "CombatantFactory".Length)
        .ToLowerInvariant();

    protected abstract CombatantStatsConfig CombatantStatsConfig();

    protected abstract CombatMovementSequence CombatMovementSequence(int variationIndex);

    public MythlandersCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories)
    {
        var monsterSequence = CombatMovementSequence(variationIndex);

        var stats = CombatantStatsConfig();

        var monster = new MythlandersCombatant(ClassSid, monsterSequence, stats, combatActorBehaviour,
            combatantStatusFactories)
        {
            DebugSid = sid, IsPlayerControlled = false
        };

        return monster;
    }
}