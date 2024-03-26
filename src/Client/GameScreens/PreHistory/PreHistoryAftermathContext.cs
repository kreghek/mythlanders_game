using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly Player _player;
    private readonly IDictionary<string, IPreHistoryBackground> _backgrounds;

    private IPreHistoryBackground? _backgroundTexture;
    
    public PreHistoryAftermathContext(IDictionary<string, IPreHistoryBackground> backgrounds, 
        IDialogueEnvironmentManager dialogueEnvironmentManager,
        Player player)
    {
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _player = player;

        _backgrounds = backgrounds;
    }

    public void SetBackground(string backgroundName)
    {
        _backgroundTexture = _backgrounds[backgroundName];
    }

    public IPreHistoryBackground? GetBackgroundTexture()
    {
        return _backgroundTexture;
    }

    public void PlaySong(string resourceName)
    {
        _dialogueEnvironmentManager.PlaySong(resourceName);
    }

    public void PlaySoundEffect(string effectSid, string resourceName)
    {
        _dialogueEnvironmentManager.PlayEffect(effectSid, resourceName);
    }

    public void AddNewHero(string heroSid)
    {
        _player.AddHero(HeroState.Create(heroSid));
    }

    internal void AddMonsterPerk(string perkSid)
    {
        var monsterPerks = CatalogHelper.GetAllFromStaticCatalog<MonsterPerk>(typeof(MonsterPerkCatalog));
        var targetPerk = monsterPerks.Single(x => string.Equals(x.Sid, perkSid, StringComparison.InvariantCultureIgnoreCase));

        _player.AddMonsterPerk(targetPerk);
    }
}