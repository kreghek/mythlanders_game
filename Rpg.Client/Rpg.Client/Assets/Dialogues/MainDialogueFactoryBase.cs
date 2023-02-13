﻿using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Dialogues
{
    internal abstract class MainDialogueFactoryBase : IDialogueFactory
    {
        protected virtual bool IsGameStart { get; }
        protected abstract string Sid { get; }

        protected virtual IReadOnlyCollection<ITextEventRequirement>? CreateRequirements(IEventCatalog eventCatalog)
        {
            return null;
        }

        public Event Create(IEventCatalog eventCatalog)
        {
            var mainPlot1 = new Event
            {
                Sid = Sid,
                IsGameStart = IsGameStart,
                IsUnique = true,
                BeforeCombatStartNodeSid = $"{Sid}_Before",
                AfterCombatStartNodeSid = $"{Sid}_After",
                Priority = TextEventPriority.High,
                Requirements = CreateRequirements(eventCatalog)
            };

            return mainPlot1;
        }
    }
}