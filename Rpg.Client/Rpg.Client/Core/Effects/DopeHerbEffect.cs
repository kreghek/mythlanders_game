using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Effects
{
    internal class DopeHerbEffect : PeriodicEffectBase
    {
        protected override void InfluenceAction()
        {
            Combat.Pass();
            base.InfluenceAction();
        }
    }
}