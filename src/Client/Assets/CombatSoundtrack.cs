﻿using Client.Engine;

using Microsoft.Xna.Framework.Media;

namespace Client.Assets;

/// <summary>
/// Metadata of combat soundtrack to select correct track in a combats.
/// </summary>
internal record CombatSoundtrack(LocationCulture Culture, Song Soundtrack, CombatSoundtrackRole SoundtrackRole)
{
    public CombatSoundtrack(LocationCulture culture, Song soundtrack) : this(culture, soundtrack,
        CombatSoundtrackRole.Regular)
    {
    }
}