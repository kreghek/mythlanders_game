using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectProcessor
    {
        private readonly Combat _combat;
        private readonly IDice _dice;
        private readonly IDictionary<CombatUnit, IList<EffectBase>> _unitEffects;

        public EffectProcessor(Combat combat, IDice dice)
        {
            _combat = combat;
            _dice = dice;
            _unitEffects = new Dictionary<CombatUnit, IList<EffectBase>>();
        }

        public IEnumerable<EffectBase> GetCurrentEffect(CombatUnit combatUnit)
        {
            if (!_unitEffects.TryGetValue(combatUnit, out var effects))
            {
                return ArraySegment<EffectBase>.Empty;
            }

            return effects.ToArray();
        }

        public void Impose(IEnumerable<EffectRule>? influences, CombatSkillEnv env, CombatUnit actor, CombatUnit? target)
        {
            if (influences is null)
            {
                return;
            }

            foreach (var influence in influences)
            {
                ImposeSingleRule(influence, env, actor, target);
            }
        }

        public void Influence(CombatUnit? combatUnit)
        {
            if (combatUnit is null || !_unitEffects.ContainsKey(combatUnit))
            {
                return;
            }

            if (combatUnit.Unit.IsDead)
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

            _unitEffects[e.Unit].Add(e.Effect);
        }

        private void ImposeByCreator(EffectCreator creator, CombatSkillEnv env, CombatUnit self, CombatUnit target)
        {
            var effect = creator.Create(self, env, _combat);

            effect.Imposed += Effect_Imposed;
            effect.Dispelled += Effect_Dispelled;

            effect.Impose(target, env);
        }

        private void ImposeSingleRule(EffectRule influence, CombatSkillEnv env, CombatUnit actor, CombatUnit? target)
        {
            switch (influence.Direction)
            {
                case SkillDirection.All:
                    foreach (var unit in _combat.AliveUnits)
                    {
                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                case SkillDirection.AllEnemies:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                case SkillDirection.AllFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                case SkillDirection.Other:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != actor))
                    {
                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                case SkillDirection.OtherFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != actor && x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                case SkillDirection.Self:
                    ImposeByCreator(influence.EffectCreator, env, actor, actor);

                    break;

                case SkillDirection.RandomEnemy:
                    {
                        var unit = _dice.RollFromList(_combat.AliveUnits
                            .Where(x => x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled).ToList());

                        ImposeByCreator(influence.EffectCreator, env, actor, unit);
                    }

                    break;

                // case SkillDirection.RandomFriendly:
                //     {
                //         var unit = dice.RollFromList(_combat.AliveUnits
                //             .Where(x => x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled).ToList());
                //
                //         ImposeByCreator(influence.EffectCreator, self, unit);
                //     }
                //
                //     break;

                case SkillDirection.Target:
                    if (target is null)
                    {
                        throw new InvalidOperationException();
                    }

                    ImposeByCreator(influence.EffectCreator, env, actor, target);

                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}