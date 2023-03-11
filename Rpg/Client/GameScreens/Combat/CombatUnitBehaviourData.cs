using System.Collections.Generic;
using System.Linq;

using Core.Combats;

namespace Rpg.Client.GameScreens.Combat
{
    public sealed class CombatUnitBehaviourData : ICombatActorBehaviourData
    {
        public CombatUnitBehaviourData(CombatCore combat)
        {
            CurrentActor =
                new CombatUnitBehaviourDataActor(
                    combat.CurrentCombatant.Hand.Where(x => x is not null).Select(skill => new CombatActorBehaviourDataSkill(skill!)).ToArray());

            Actors = combat.Field.HeroSide.GetAllCombatants().Concat(combat.Field.MonsterSide.GetAllCombatants()).Where(actor => actor != combat.CurrentCombatant).Select(actor =>
                    new CombatUnitBehaviourDataActor(
                        actor.Hand.Where(x => x is not null).Select(skill => new CombatActorBehaviourDataSkill(skill!)).ToArray()))
                .ToArray();
        }

        public CombatUnitBehaviourDataActor CurrentActor { get; }
        public IReadOnlyCollection<CombatUnitBehaviourDataActor> Actors { get; }
    }
}