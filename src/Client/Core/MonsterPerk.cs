using System.Collections.Generic;

using Client.Assets.CombatMovements;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

internal sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid, IReadOnlyCollection<CombatMovementEffectDisplayValue> Values);