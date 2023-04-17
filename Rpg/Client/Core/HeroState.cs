using Core.Combats;

namespace Client.Core;

internal sealed record HeroState(string ClassSid, IStatValue HitPoints, FieldCoords FormationPosition);