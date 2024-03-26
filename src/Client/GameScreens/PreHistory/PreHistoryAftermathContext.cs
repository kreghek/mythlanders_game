using System;
using System.Linq;

using Client.Assets;
using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dialogues;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly ContentManager _contentManager;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly Player _player;
    private Texture2D? _backgroundTexture;

    public PreHistoryAftermathContext(ContentManager contentManager,
        IDialogueEnvironmentManager dialogueEnvironmentManager, Player player)
    {
        _contentManager = contentManager;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _player = player;
    }

    public void AddNewHero(string heroSid)
    {
        _player.AddHero(HeroState.Create(heroSid));
    }

    public Texture2D? GetBackgroundTexture()
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
        _backgroundTexture = _contentManager.Load<Texture2D>($"Sprites/GameObjects/PreHistory/{backgroundName}");
    }

    internal void AddMonsterPerk(string perkSid)
    {
        var monsterPerks = CatalogHelper.GetAllFromStaticCatalog<MonsterPerk>(typeof(MonsterPerkCatalog));
        var targetPerk =
            monsterPerks.Single(x => string.Equals(x.Sid, perkSid, StringComparison.InvariantCultureIgnoreCase));

        _player.AddMonsterPerk(targetPerk);
    }
}