using System;
using System.Collections.Generic;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Assets.DialogueOptionAftermath;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal class StaticTextEventCatalog : IEventCatalog, IEventInitializer
    {
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private bool _isInitialized;

        private IDictionary<string, Dialogue>? _nodes;

        public StaticTextEventCatalog(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;
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
            
            Events = events;

            var mainPlot1 = new Event
            {
                Sid = "MainSlavic1",
                IsGameStart = true,
                IsUnique = true,
                BeforeCombatStartNodeSid = "MainSlavic1Before"
            };

            events.Add(mainPlot1);

            var mainPlot2 = new Event
            {
                Sid = "MainSlavic2",
                IsUnique = true,
                BeforeCombatStartNodeSid = "MainSlavic2Before",
                Requirements = new[]
                {
                    new LocationEventRequirement(new[] {
                        GlobeNodeSid.Battleground
                    })
                }
            };
            
            events.Add(mainPlot2);

            var mainSlavic1BeforeDialogue = CreateMainSlavic1BeforeDialogue();
            var mainSlavic2BeforeDialogue = CreateMainSlavic2BeforeDialogue();

            _nodes = new Dictionary<string, Dialogue>
            {
                { "MainSlavic1Before", mainSlavic1BeforeDialogue },
                { "MainSlavic2Before", mainSlavic2BeforeDialogue }
            };

            _isInitialized = true;
        }

        private static Dialogue CreateMainSlavic1BeforeDialogue()
        {
            var mainSlavic1BeforeRoot = new EventNode
            {
                TextBlock = new EventTextBlock
                {
                    Fragments = new[]
                    {
                        new EventTextFragment
                        {
                            Speaker = UnitName.Environment, TextSid = "MainSlavic1Before_01_Text"
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
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic1Before_02"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic1Before_02_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new UnlockLocationOptionAftermath(GlobeNodeSid.Battleground)
                            }
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
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic1Before_03"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic1Before_03_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new UnlockLocationOptionAftermath(GlobeNodeSid.Battleground)
                            }
                        }
                    })
                }
            };

            var mainSlavic1BeforeDialogue = new Dialogue(mainSlavic1BeforeRoot, EventPosition.BeforeCombat);

            return mainSlavic1BeforeDialogue;
        }
        
        private Dialogue CreateMainSlavic2BeforeDialogue()
        {
            var mainSlavic1BeforeRoot = new EventNode
            {
                TextBlock = new EventTextBlock
                {
                    Fragments = new[]
                    {
                        new EventTextFragment
                        {
                            Speaker = UnitName.Environment, TextSid = "MainSlavic2Before_01_Text"
                        }
                    }
                },
                Options = new[]
                {
                    new EventOption("MainSlavic2Before_01_Option_01", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic2Before_02"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic2Before_02_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddPlayerCharacterOptionAftermath(_unitSchemeCatalog.Heroes[UnitName.Archer])
                            }
                        }
                    }),
                    new EventOption("MainSlavic2Before_01_Option_02", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic2Before_03"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic2Before_03_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddPlayerCharacterOptionAftermath(_unitSchemeCatalog.Heroes[UnitName.Archer])
                            }
                        }
                    })
                }
            };

            var mainSlavic1BeforeDialogue = new Dialogue(mainSlavic1BeforeRoot, EventPosition.BeforeCombat);
            return mainSlavic1BeforeDialogue;
        }
    }
}