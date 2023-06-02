using Microsoft.Xna.Framework.Media;

using Rpg.Client.Engine;

namespace Client.Assets;

internal record CombatSoundtrack(LocationCulture Culture, Song Soundtrack, CombatSoundtrackRole SoundtrackRole)
{
    public CombatSoundtrack(LocationCulture culture, Song soundtrack):this(culture, soundtrack, CombatSoundtrackRole.Regular)
    {
    }
}
