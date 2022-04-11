﻿using System;

namespace Rpg.Client.Core.SkillEffects
{
    internal class StunEffect : PeriodicEffectBase
    {
        public override void MergeWithBase(EffectBase testedEffect)
        {
            if (testedEffect is StunEffect stunEffect)
            {
                stunEffect.Duration += Duration;
            }
            else
            {
                throw new InvalidOperationException("can be merged only with stun effect.");
            }
        }

        public override bool CanBeMerged(EffectBase testedEffect)
        {
            return testedEffect is StunEffect;
        }

        protected override void InfluenceAction()
        {
            Combat.Pass();
            base.InfluenceAction();
        }
    }
}