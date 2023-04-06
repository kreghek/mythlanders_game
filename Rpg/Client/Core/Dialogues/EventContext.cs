using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Dialogues;
using Client.Core.Heroes;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class EventContext : IEventContext
    {
        private readonly Globe _globe;
        private readonly Player _player;
        private readonly IStoryPointCatalog _storyPointCatalog;

        public EventContext(Globe globe, IStoryPointCatalog storyPointCatalog, Player player,
            DialogueEvent currentDialogueEvent)
        {
            _globe = globe;
            _storyPointCatalog = storyPointCatalog;
            _player = player;

            CurrentDialogueEvent = currentDialogueEvent;

            CurrentHeroes = player.Heroes.Select(x => x.ClassSid).ToArray();
        }

        public DialogueEvent CurrentDialogueEvent { get; }

        public void AddNewCharacter(Hero unit)
        {
            var freeSlots = _globe.Player.Party.GetFreeSlots()
                .Where(
                    x => BoolHelper.HasNotRestriction(_player.HasAbility(PlayerAbility.AvailableTanks), x.IsTankLine))
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

        public void AddNewGlobalEvent(IGlobeEvent globalEvent)
        {
            _globe.AddGlobalEvent(globalEvent);
        }

        public void AddStoryPoint(string storyPointSid)
        {
            var storyPoint = _storyPointCatalog.GetAll().Single(x => x.Sid == storyPointSid);
            _globe.AddActiveStoryPoint(storyPoint);
        }

        public void StartCombat(string sid)
        {
            throw new NotImplementedException();
        }

        public void UnlockLocation(LocationSid locationSid)
        {
            //_globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == locationSid).IsAvailable = true;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> CurrentHeroes { get; }
    }
}