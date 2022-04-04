using System.Collections.Generic;

using Rpg.Client.Core.Modifiers;
using Rpg.Client.Core.SkillEffects;

namespace Rpg.Client.Core
{
    internal interface ICombat
    {
        IEnumerable<ICombatUnit> AliveUnits { get; }
        EffectProcessor EffectProcessor { get; }
        ModifiersProcessor ModifiersProcessor { get; }
        IDice Dice { get; }
        void Pass();
    }
}