using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;
using Client.GameScreens.Common.GlobeNotifications;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly IDictionary<string, IPreHistoryScene> _backgrounds;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly GlobeNotificationFactory _globeNotificationFactory;
    private readonly IGlobeNotificationManager _globeNotificationManager;
    private readonly IMonsterPerkCatalog _monsterPerkCatalog;
    private readonly Player _player;

    private IPreHistoryScene? _backgroundTexture;

    public PreHistoryAftermathContext(IDictionary<string, IPreHistoryScene> backgrounds,
        IDialogueEnvironmentManager dialogueEnvironmentManager,
        Player player,
        IMonsterPerkCatalog monsterPerkCatalog,
        IGlobeNotificationManager globeNotificationManager,
        GlobeNotificationFactory globeNotificationFactory)
    {
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _player = player;
        _monsterPerkCatalog = monsterPerkCatalog;
        _globeNotificationManager = globeNotificationManager;
        _globeNotificationFactory = globeNotificationFactory;
        _backgrounds = backgrounds;
    }

    public void AddNewHero(string heroSid)
    {
        _player.AddHero(HeroState.Create(heroSid));

        _globeNotificationManager.AddNotification(
            _globeNotificationFactory.Create((provider, monsterPerkTexture) =>
                new NewHeroJoinedGlobeNotification(heroSid, provider.Get(heroSid))));
    }

    public IPreHistoryScene? GetBackgroundTexture()
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

        _globeNotificationManager.AddNotification(
            _globeNotificationFactory.Create((provider, monsterPerkTexture) =>
                new MonsterPerkAddedGlobeNotification(targetPerk, monsterPerkTexture)));
    }

    internal void UnlockLocation(ILocationSid locationSid)
    {
        _player.AddLocation(locationSid);
    }
}