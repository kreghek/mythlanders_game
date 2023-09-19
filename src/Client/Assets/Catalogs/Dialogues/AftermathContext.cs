using System;
using System.Linq;

using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.Heroes;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

internal class AftermathContext
{
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;

    public AftermathContext(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        DialogueEvent currentDialogueEvent, IDialogueEnvironmentManager dialogueEnvironmentManager)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;

        CurrentDialogueEvent = currentDialogueEvent;
    }

    public DialogueEvent CurrentDialogueEvent { get; }

    public void AddNewCharacter(Hero unit)
    {
        var freeSlots = _globe.Player.Party.GetFreeSlots()
            //.Where(
            //    x => BoolHelper.HasNotRestriction(_player.HasAbility(PlayerAbility.AvailableTanks), x.IsTankLine))
            .ToArray();
        if (freeSlots.Any())
        {
            var selectedFreeSlot = freeSlots.First();
            _globe.Player.MoveToParty(unit, selectedFreeSlot.Index);
        }
        else
        {
            _globe.Player.Pool.AddNewUnit(unit);
        }
    }

    public void AddNewGlobalEvent(CharacterDeepPreyingGlobeEvent globalEvent)
    {
        _globe.AddGlobalEvent(globalEvent);
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

    public void PlaySong(string resourceName)
    {
        _dialogueEnvironmentManager.PlaySong(resourceName);
    }

    public void PlaySoundEffect(string effectSid, string resourceName)
    {
        _dialogueEnvironmentManager.PlayEffect(effectSid, resourceName);
    }

    public void StartCombat(string sid)
    {
        throw new NotImplementedException();
    }

    public void UnlockLocation(ILocationSid locationSid)
    {
        throw new NotImplementedException();
    }
}