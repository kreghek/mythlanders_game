using System;
using System.Collections.Generic;

using Client.Assets;

using Microsoft.Xna.Framework.Media;

namespace Rpg.Client.Engine
{
    internal struct CombatSoundtrack
    {
        public LocationCulture ApplicableBiome;
        public CombatSoundtrackRole Role;
        public Song Soundtrack;

        public CombatSoundtrack(LocationCulture applicableBiome, CombatSoundtrackRole role, Song soundtrack)
        {
            ApplicableBiome = applicableBiome;
            Role = role;
            Soundtrack = soundtrack;
        }

        public CombatSoundtrack(LocationCulture applicableBiome, Song soundtrack) : this(applicableBiome,
            CombatSoundtrackRole.Regular, soundtrack)
        {
        }

        public override bool Equals(object? obj)
        {
            return obj is CombatSoundtrack other &&
                   ApplicableBiome == other.ApplicableBiome &&
                   Role == other.Role &&
                   EqualityComparer<Song>.Default.Equals(Soundtrack, other.Soundtrack);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ApplicableBiome, Role, Soundtrack);
        }
    }
}