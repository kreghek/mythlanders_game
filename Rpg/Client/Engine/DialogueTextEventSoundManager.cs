using System;
using System.Collections.Generic;

using Client.Core.Dialogues;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Engine;

namespace Client.Engine;

internal sealed class DialogueTextEventSoundManager : IDialogueTextEventSoundManager
{
    private readonly IDictionary<string, SoundEffect> _effectsDict;
    private readonly IDictionary<string, Song> _musicDict;
    private readonly IDictionary<string, SoundEffectInstance> _currentEffects;
    private readonly SoundtrackManager _soundtrackManager;

    public DialogueTextEventSoundManager(SoundtrackManager soundtrackManager)
    {
        _effectsDict = new Dictionary<string, SoundEffect>();
        _musicDict = new Dictionary<string, Song>();
        _currentEffects = new Dictionary<string, SoundEffectInstance>();
        _soundtrackManager = soundtrackManager;
    }

    public void Init(ContentManager content)
    {
        _effectsDict.Add("DesertWind", content.Load<SoundEffect>("Audio/Stories/DesertWind"));
        _effectsDict.Add("WomanSynthCrying", content.Load<SoundEffect>("Audio/Stories/WomanSynthCrying"));

        _musicDict.Add("EgyptianThriller", content.Load<Song>("Audio/Stories/EgyptianThrillerMusic"));
    }

    public void PlayEffect(string effectSid, string resourceName)
    {
        var effectInstanse = _effectsDict[resourceName].CreateInstance();
        effectInstanse.Play();
        _currentEffects.Add(effectSid, effectInstanse);
    }

    public void PlaySong(string resourceName)
    {
        var song = _musicDict[resourceName];
        _soundtrackManager.PlayCustomTrack(song);
    }
}
