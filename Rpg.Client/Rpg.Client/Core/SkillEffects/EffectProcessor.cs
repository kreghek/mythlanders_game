using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectProcessor
    {
        private readonly ICombat _combat;
        private readonly IDice _dice;
        private readonly IDictionary<ICombatUnit, IList<EffectBase>> _unitEffects;

        public EffectProcessor(ICombat combat, IDice dice)
        {
            _combat = combat;
            _dice = dice;
            _unitEffects = new Dictionary<ICombatUnit, IList<EffectBase>>();
        }

        public IEnumerable<EffectBase> GetCurrentEffect(ICombatUnit combatUnit)
        {
            if (!_unitEffects.TryGetValue(combatUnit, out var effects))
            {
                return ArraySegment<EffectBase>.Empty;
            }

            return effects.ToArray();
        }

        public IReadOnlyList<ICombatUnit> GetTargets(EffectRule influence, ICombatUnit actor, ICombatUnit target)
        {
            switch (influence.Direction)
            {
                case SkillDirection.All:
                    return _combat.AliveUnits.ToArray();

                case SkillDirection.AllEnemies:
                    return _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled).ToArray();

                case SkillDirection.AllTankingEnemies:
                    return GetAllTankingEnemies(actor);

                case SkillDirection.AllFriendly:
                    return _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled).ToArray();

                case SkillDirection.Other:
                    return _combat.AliveUnits.Where(x => x != actor).ToArray();

                case SkillDirection.OtherFriendly:
                    return _combat.AliveUnits.Where(x =>
                        x != actor && x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled).ToArray();

                case SkillDirection.Self:
                    return new[] { actor };

                case SkillDirection.RandomEnemy:
                    {
                        var unit = _dice.RollFromList(_combat.AliveUnits
                            .Where(x => x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled).ToList());

                        return new[] { unit };
                    }

                case SkillDirection.RandomFriendly:
                    {
                        var unit = _dice.RollFromList(_combat.AliveUnits
                            .Where(x => x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled).ToList());

                        return new[] { unit };
                    }

                case SkillDirection.Target:
                    if (target is null)
                    {
                        throw new InvalidOperationException();
                    }

                    return new[] { target };

                default:
                    throw new InvalidOperationException();
            }
        }

        public void Impose(IEnumerable<EffectRule>? influences, ICombatUnit actor, ICombatUnit? target,
            IEffectSource effectSource)
        {
            if (influences is null)
            {
                return;
            }

            foreach (var influence in influences)
            {
                ImposeSingleRule(influence.EffectCreator, actor, target, effectSource);
            }
        }

        public void Influence(ICombatUnit? combatUnit)
        {
            if (combatUnit is null || !_unitEffects.ContainsKey(combatUnit))
            {
                return;
            }

            if (combatUnit.IsDead)
            {
                if (_unitEffects.ContainsKey(combatUnit))
                {
                    _unitEffects.Remove(combatUnit);
                    return;
                }
            }

            var effects = new List<EffectBase>(_unitEffects[combatUnit]);

            foreach (var effect in effects)
            {
                effect.Influence();
            }
        }

        private void Effect_Dispelled(object? sender, EffectBase.UnitEffectEventArgs e)
        {
            if (!_unitEffects.ContainsKey(e.Unit))
            {
                return;
            }

            _unitEffects[e.Unit].Remove(e.Effect);
        }

        private void Effect_Imposed(object? sender, EffectBase.UnitEffectEventArgs e)
        {
            if (!_unitEffects.ContainsKey(e.Unit))
            {
                _unitEffects[e.Unit] = new List<EffectBase>();
            }

            e.Effect.AddToList(_unitEffects[e.Unit]);
        }

        private ICombatUnit[] GetAllTankingEnemies(ICombatUnit actor)
        {
            // 1. Attack units on tanking line first.
            // 2. Attack back line unit if there are no tanks  

            var tankingUnits = _combat.AliveUnits.Where(x =>
                    x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled &&
                    ((CombatUnit)actor).IsInTankLine)
                .ToArray();

            if (!tankingUnits.Any())
            {
                tankingUnits = _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled)
                    .ToArray();
            }

            return tankingUnits;
        }

        private void ImposeByCreator(EffectCreator creator, ICombatUnit self, ICombatUnit target,
            IEffectSource effectSource)
        {
            var effect = creator.Create(self, _combat, effectSource);

            effect.Imposed += Effect_Imposed;
            effect.Dispelled += Effect_Dispelled;

            effect.Impose(target);
        }

        private void ImposeSingleRule(EffectCreator effectCreator, ICombatUnit actor, ICombatUnit materializedTarget,
            IEffectSource effectSource)
        {
            ImposeByCreator(effectCreator, actor, materializedTarget, effectSource);
        }
    }
}