using System.Collections.Generic;

using CombatDicesTeam.Dialogues;

using Core.Crises;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Client.Engine;

internal sealed class DialogueEnvironmentManager : IDialogueEnvironmentManager
{
    private readonly IDictionary<string, SoundEffectInstance> _currentEffects;
    private readonly IDictionary<string, SoundEffect> _effectsDict;
    private readonly IDictionary<string, Song> _musicDict;
    private readonly SoundtrackManager _soundtrackManager;

    public DialogueEnvironmentManager(SoundtrackManager soundtrackManager)
    {
        _effectsDict = new Dictionary<string, SoundEffect>();
        _musicDict = new Dictionary<string, Song>();
        _currentEffects = new Dictionary<string, SoundEffectInstance>();
        _soundtrackManager = soundtrackManager;
    }

    public void Init(ContentManager content)
    {
        _effectsDict.Add("DesertWind", content.Load<SoundEffect>("Audio/Stories/Sounds/DesertWind"));
        _effectsDict.Add("WomanSynthCrying", content.Load<SoundEffect>("Audio/Stories/Sounds/WomanSynthCrying"));
        _effectsDict.Add("ChineseCrowd", content.Load<SoundEffect>("Audio/Stories/Sounds/ChineseCrowd"));

        _musicDict.Add("EgyptianThriller", content.Load<Song>("Audio/Stories/Music/EgyptianThrillerMusic"));
        _musicDict.Add("ChineseHappy", content.Load<Song>("Audio/Stories/Music/ChineseHappyMusic"));
        _musicDict.Add("ChineseMeditation", content.Load<Song>("Audio/Stories/Music/ChineseMeditationMusic"));
        
        _musicDict.Add("MagicTrap", content.Load<Song>("Audio/Stories/Music/ElectricDeathRay"));
        _musicDict.Add("CityHunting", content.Load<Song>("Audio/Stories/Music/CityHunting"));
        _musicDict.Add("InfernalSickness", content.Load<Song>("Audio/Stories/Music/InfernalSickness"));
        _musicDict.Add("Starvation", content.Load<Song>("Audio/Stories/Music/Starvation"));
        _musicDict.Add("Preying", content.Load<Song>("Audio/Stories/Music/SkyThunder"));
    }

    public void PlayEffect(string effectSid, string resourceName)
    {
        if (_currentEffects.ContainsKey(resourceName))
        {
            _currentEffects[effectSid].Stop();
            _currentEffects.Remove(effectSid);
        }

        var effectInstance = _effectsDict[resourceName].CreateInstance();
        _currentEffects.Add(effectSid, effectInstance);

        effectInstance.Play();
    }

    public void PlaySong(string resourceName)
    {
        var song = _musicDict[resourceName];
        _soundtrackManager.PlayCustomTrack(song);
    }

    public void Clean()
    {
        foreach (var effect in _currentEffects)
        {
            effect.Value.Stop();
        }

        _currentEffects.Clear();
        _soundtrackManager.PlaySilence();
    }
    
    private static string GetBackgroundEffectName(CrisisSid sid)
    {
        return sid.Value switch
        {
            "MagicTrap" => "ElectricDeathRay",
            "CityHunting" => "CityHunting",
            "InfernalSickness" => "InfernalSickness",
            "Starvation" => "Starvation",
            "Preying" => "SkyThunder",
            _ => "Starvation"
        };
    }
}