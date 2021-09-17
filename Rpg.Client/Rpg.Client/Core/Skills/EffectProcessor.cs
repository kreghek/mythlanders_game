using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core.Skills
{
    internal class EffectProcessor
    {
        private readonly ActiveCombat _combat;
        private readonly IDice _dice;

        public EffectProcessor(ActiveCombat combat, IDice dice)
        {
            _combat = combat;
            _dice = dice;
        }

        public void Influence(IEnumerable<EffectRule> influences, CombatUnit self, CombatUnit? target)
        {
            if (influences is null)
                return;

            foreach (var influence in influences)
            {
                Influence(influence, self, target);
            }
        }

        private void Impose(EffectCreator creator, CombatUnit self, CombatUnit target)
        {
            var effect = creator.Create(self, this, _dice);

            effect.Impose(target);
        }

        private void Influence(EffectRule influence, CombatUnit self, CombatUnit? target)
        {
            var dice = new LinearDice(DateTime.Now.Millisecond);

            switch (influence.Direction)
            {
                case SkillDirection.All:
                    foreach (var unit in _combat.AliveUnits)
                    {
                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.AllEnemy:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled != self.Unit.IsPlayerControlled))
                    {
                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.AllFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled))
                    {
                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.Other:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != self))
                    {
                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.OtherFriendly:
                    foreach (var unit in _combat.AliveUnits.Where(x =>
                        x != self && x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled))
                    {
                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.Self:
                    Impose(influence.EffectCreator, self, self);

                    break;

                case SkillDirection.RandomEnemy:
                    {
                        var unit = dice.RollFromList(_combat.AliveUnits
                            .Where(x => x.Unit.IsPlayerControlled != self.Unit.IsPlayerControlled).ToList());

                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.RandomFriendly:
                    {
                        var unit = dice.RollFromList(_combat.AliveUnits
                            .Where(x => x.Unit.IsPlayerControlled == self.Unit.IsPlayerControlled).ToList());

                        Impose(influence.EffectCreator, self, unit);
                    }

                    break;

                case SkillDirection.Target:
                    if (target is null)
                        throw new InvalidOperationException();

                    Impose(influence.EffectCreator, self, target);

                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}