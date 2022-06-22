using System;
using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal class StaticTextEventCatalog : IEventCatalog, IEventInitializer
    {
        private bool _isInitialized;

        private IDictionary<string, Dialogue>? _nodes;

        public StaticTextEventCatalog()
        {
            _isInitialized = false;
            Events = Array.Empty<Event>();
        }

        public IEnumerable<Event> Events { get; private set; }

        public Dialogue GetDialogue(string sid)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException();
            }

            if (_nodes is null)
            {
                throw new InvalidOperationException();
            }

            return _nodes[sid];
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
                    new EventOption("MainSlavic1Before_01_Option_01", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment,
                                    TextSid = "MainSlavic1Before_02"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic1Before_02_Option_01", EventNode.EndNode)
                        }
                    }),
                    new EventOption("MainSlavic1Before_01_Option_02", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment,
                                    TextSid = "MainSlavic1Before_03"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic1Before_03_Option_01", EventNode.EndNode)
                        }
                    })
                }
            };

            var mainSlavicBefore1Dialogue = new Dialogue(mainSlavic1BeforeRoot, EventPosition.BeforeCombat);

            _nodes = new Dictionary<string, Dialogue>
            {
                { "MainSlavic1Before", mainSlavicBefore1Dialogue }
            };

            _isInitialized = true;
        }
    }
}