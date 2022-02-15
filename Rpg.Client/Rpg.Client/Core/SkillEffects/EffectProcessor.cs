using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.SkillEffects
{
    internal class EffectProcessor
    {
        private readonly ICombat _combat;

        private readonly IDictionary<ICombatUnit, IList<EffectBase>> _unitEffects;

        public EffectProcessor(ICombat combat)
        {
            _combat = combat;
            _unitEffects = new Dictionary<ICombatUnit, IList<EffectBase>>();
        }

        public IEnumerable<EffectBase> GetCurrentEffect(CombatUnit combatUnit)
        {
            if (!_unitEffects.TryGetValue(combatUnit, out var effects))
            {
                return ArraySegment<EffectBase>.Empty;
            }

            return effects.ToArray();
        }

        public void Impose(IEnumerable<EffectRule>? influences, ICombatUnit self, ICombatUnit? target)
        {
            if (influences is null)
            {
                return;
            }

            foreach (var influence in influences)
            {
                ImposeSingleRule(influence, self, target);
            }
        }

        public void Influence(ICombatUnit? combatUnit)
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

        private void ImposeByCreator(EffectCreator creator, ICombatUnit self, ICombatUnit target)
        {
            var effect = creator.Create(self, _combat);

            effect.Imposed += Effect_Imposed;
            effect.Dispelled += Effect_Dispelled;

            effect.Impose(target);
        }

        private void ImposeSingleRule(EffectRule influence, ICombatUnit self, ICombatUnit? target)
        {
            switch (influence.Direction)
            {
                case SkillDirection.All:
                    foreach (var unit in _combat.AliveUnits)
                    {
                        ImposeByCreator(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.AllEnemy:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled != self.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.AllFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.Other:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != self))
                    {
                        ImposeByCreator(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.OtherFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != self && x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled))
                    {
                        ImposeByCreator(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.Self:
                    ImposeByCreator(influence.EffectCreator, self, self);

                    break;

                // case SkillDirection.RandomEnemy:
                //     {
                //         var unit = dice.RollFromList(_combat.AliveUnits
                //             .Where(x => x.Unit.IsPlayerControlled != self.Unit.IsPlayerControlled).ToList());
                //
                //         ImposeByCreator(influence.EffectCreator, self, unit);
                //     }
                //
                //     break;
                //
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

                    ImposeByCreator(influence.EffectCreator, self, target);

                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}