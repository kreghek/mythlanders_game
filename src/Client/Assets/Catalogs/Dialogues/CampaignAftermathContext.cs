using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Heroes;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dialogues;

using Core.Props;

namespace Client.Assets.Catalogs.Dialogues;

internal class CampaignAftermathContext
{
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly HeroCampaign _heroCampaign;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public CampaignAftermathContext(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        DialogueEvent currentDialogueEvent, IDialogueEnvironmentManager dialogueEnvironmentManager,
        HeroCampaign heroCampaign)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
        _heroCampaign = heroCampaign;

        CurrentDialogueEvent = currentDialogueEvent;
    }

    public DialogueEvent CurrentDialogueEvent { get; }

    public void AddNewHero(Hero unit)
    {
        _globe.Player.Heroes.AddNewUnit(new HeroState(unit.UnitScheme.Name.ToString(), new StatValue(3)));
    }

    public void AddNewGlobalEvent(CharacterDeepPreyingGlobeEvent globalEvent)
    {
        _globe.AddGlobalEvent(globalEvent);
    }

    public void AddResources(IProp resource)
    {
        _player.Inventory.Add(resource);
    }

    public void AddStoryPoint(string storyPointSid)
    {
        var storyPoint = _storyPointCatalog.GetAll().Single(x => x.Sid == storyPointSid);
        _globe.AddActiveStoryPoint(storyPoint);
    }

    public void ChangeCharacterRelations(UnitName targetCharacter, CharacterKnowledgeLevel knowledgeLevel)
    {
        var dialogueSpeaker = DialogueSpeakers.Get(targetCharacter);

        var relation =
            _player.StoryState.CharacterRelations.SingleOrDefault(x =>
                x.Character == dialogueSpeaker);

        if (relation is null)
        {
            _player.StoryState.AddCharacterRelations(targetCharacter);

            relation =
                _player.StoryState.CharacterRelations.Single(x =>
                    x.Character == dialogueSpeaker);
        }

        relation.Level = knowledgeLevel;
    }

    public void DamageHero(string heroClassSid, int damageAmount)
    {
        var hero = _heroCampaign.Heroes.Single(x => x.State.ClassSid == heroClassSid);
        hero.State.HitPoints.Consume(damageAmount);
        HeroHpChanged?.Invoke(this, new HeroStatChangedEventArgs(heroClassSid, -damageAmount));
    }

    public IReadOnlyCollection<string> GetPartyHeroes()
    {
        return _heroCampaign.Heroes.Where(x => x.State.HitPoints.Current > 0).Select(x => x.State.ClassSid).ToArray();
    }

    public IReadOnlyCollection<string> GetWoundedHeroes()
    {
        return _heroCampaign.Heroes.Where(x => x.State.HitPoints.Current <= 0).Select(x => x.State.ClassSid).ToArray();
    }

    public void PlaySong(string resourceName)
    {
        _dialogueEnvironmentManager.PlaySong(resourceName);
    }

    public void PlaySoundEffect(string effectSid, string resourceName)
    {
        _dialogueEnvironmentManager.PlayEffect(effectSid, resourceName);
    }

    public void RemoveResource(IProp resource)
    {
        _player.Inventory.Remove(resource);
    }

    public void RestHero(string heroClassSid, int healAmount)
    {
        var hero = _heroCampaign.Heroes.Single(x => x.State.ClassSid == heroClassSid);
        hero.State.HitPoints.Restore(healAmount);
        HeroHpChanged?.Invoke(this, new HeroStatChangedEventArgs(heroClassSid, healAmount));
    }

    public void StartCombat(string sid)
    {
        throw new NotImplementedException();
    }

    public void UnlockLocation(ILocationSid locationSid)
    {
        throw new NotImplementedException();
    }

    public event EventHandler<HeroStatChangedEventArgs>? HeroHpChanged;
}