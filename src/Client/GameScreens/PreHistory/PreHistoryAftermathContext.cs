﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly Player _player;
    private readonly MonsterPerkCatalog _monsterPerkCatalog;
    private readonly IDictionary<string, IPreHistoryBackground> _backgrounds;

    private IPreHistoryBackground? _backgroundTexture;

    public PreHistoryAftermathContext(IDictionary<string, IPreHistoryBackground> backgrounds, 
        IDialogueEnvironmentManager dialogueEnvironmentManager,
        Player player,
        MonsterPerkCatalog monsterPerkCatalog)
    {
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _player = player;
        _monsterPerkCatalog = monsterPerkCatalog;
        _backgrounds = backgrounds;
    }

    public void AddNewHero(string heroSid)
    {
        _player.AddHero(HeroState.Create(heroSid));
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

    public void SetBackground(string backgroundName)
    {
        _backgroundTexture = _backgrounds[backgroundName];
    }

    internal void AddMonsterPerk(string perkSid)
    {
        var monsterPerks = _monsterPerkCatalog.Perks;
        var targetPerk =
            monsterPerks.Single(x => string.Equals(x.Sid, perkSid, StringComparison.InvariantCultureIgnoreCase));

        _player.AddMonsterPerk(targetPerk);
    }
}