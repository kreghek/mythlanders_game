using System.Linq;

using Client.Assets.GlobalEffects;
using Client.Core.Heroes;

namespace Client.Core.Dialogues;

internal class AftermathContext
{
    private readonly Globe _globe;
    private readonly Player _player;
    private readonly IStoryPointCatalog _storyPointCatalog;
    
    public AftermathContext(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
        DialogueEvent currentDialogueEvent)
    {
        _globe = globe;
        _storyPointCatalog = storyPointCatalog;
        _player = player;
    }

    public DialogueEvent CurrentDialogueEvent { get; }

    public void AddStoryPoint(string storyPointSid)
    {
        var storyPoint = _storyPointCatalog.GetAll().Single(x => x.Sid == storyPointSid);
        _globe.AddActiveStoryPoint(storyPoint);
    }

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

    public void ChangeCharacterRelations(UnitName targetCharacter, CharacterKnowledgeLevel knowledgeLevel)
    {
        throw new System.NotImplementedException();
    }

    public void StartCombat(string sid)
    {
        throw new System.NotImplementedException();
    }

    public void AddNewGlobalEvent(CharacterDeepPreyingGlobeEvent globalEvent)
    {
        _globe.AddGlobalEvent(globalEvent);
    }

    public void UnlockLocation(ILocationSid locationSid)
    {
        throw new System.NotImplementedException();
    }
}