using System.Collections.Generic;

using Client.Core.SkillEffects;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Modifiers;

namespace Client.Core;

internal interface ICombat
{
    IEnumerable<ICombatUnit> AliveUnits { get; }
    IDice Dice { get; }
    EffectProcessor EffectProcessor { get; }
    ModifiersProcessor ModifiersProcessor { get; }
    void Pass();
}