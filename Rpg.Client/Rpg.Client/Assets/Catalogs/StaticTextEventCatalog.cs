using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal class StaticTextEventCatalog : IEventCatalog, IEventInitializer
    {
        private bool _isInitilized;

        private IDictionary<string, EventNode>? nodes;

        public StaticTextEventCatalog()
        {
            _isInitilized = false;
            Events = Array.Empty<Event>();
        }

        public IEnumerable<Event> Events { get; private set; }

        public EventNode GetDialogRoot(string sid)
        {
            if (!_isInitilized)
            {
                throw new InvalidOperationException();
            }

            if (nodes is null)
            {
                throw new InvalidOperationException();
            }

            return nodes[sid];
        }

        public void Init()
        {
            var events = new List<Event>();

            var mainPlot1 = new Event
            {
                Sid = "MainSlavic1",
                IsGameStart = true,
                BeforeCombatStartNodeSid = "MainSlavic1Before"
            };

            events.Add(mainPlot1);

            Events = events;

            var mainSlavic1BeforeRoot = new EventNode
            {
                TextBlock = new EventTextBlock
                {
                    Fragments = new[]
                    {
                        new EventTextFragment
                        {
                            Speaker = UnitName.Environment,
                            TextSid = "MainSlavic1Before_01_Text"
                        }
                    }
                },
                Options = new[]
                {
                    new EventOption
                    {
                        TextSid = "MainSlavic1Before_01_Option_01",
                        Next = new EventNode
                        {
                            TextBlock = new EventTextBlock
                            {
                                Fragments = new[]
                                {
                                    new EventTextFragment
                                    {
                                        Speaker = UnitName.Bear,
                                        TextSid = "MainSlavic1Before_02"
                                    }
                                }
                            },
                            Options = new[]
                            {
                                new EventOption
                                {
                                    TextSid = "MainSlavic1Before_02_Option_01",
                                    IsEnd = true
                                }
                            }
                        }
                    }
                }
            };

            nodes = new Dictionary<string, EventNode>
            {
                { "MainSlavic1Before", mainSlavic1BeforeRoot }
            };

            _isInitilized = true;
        }
    }
}