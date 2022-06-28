﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

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
                            Speaker = UnitName.Archer, TextSid = "MainSlavic2Before_01_Text"
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
                                Aftermath = new AddPlayerCharacterOptionAftermath(
                                    _unitSchemeCatalog.Heroes[UnitName.Archer])
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
                                Aftermath = new AddPlayerCharacterOptionAftermath(
                                    _unitSchemeCatalog.Heroes[UnitName.Archer])
                            }
                        }
                    })
                }
            };

            var mainSlavic1BeforeDialogue = new Dialogue(mainSlavic1BeforeRoot, EventPosition.BeforeCombat);
            return mainSlavic1BeforeDialogue;
        }

        private Dialogue CreateMainSlavic3AfterDialogue()
        {
            var mainSlavic1AfterRoot = new EventNode
            {
                TextBlock = new EventTextBlock
                {
                    Fragments = new[]
                    {
                        new EventTextFragment
                        {
                            Speaker = UnitName.Assaulter, TextSid = "MainSlavic3After_01_Text"
                        }
                    }
                },
                Options = new[]
                {
                    new EventOption("MainSlavic3After_01_Option_01", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Swordsman, TextSid = "MainSlavic3After_02"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic3After_02_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddStoryPointOptionAftermath("1")
                            }
                        }
                    }),
                    new EventOption("MainSlavic3After_01_Option_02", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Archer, TextSid = "MainSlavic3After_03"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic3After_03_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddStoryPointOptionAftermath("1")
                            }
                        }
                    })
                }
            };

            var mainSlavic1AfterDialogue = new Dialogue(mainSlavic1AfterRoot, EventPosition.AfterCombat);
            return mainSlavic1AfterDialogue;
        }

        private Dialogue CreateMainSlavic3BeforeDialogue()
        {
            var mainSlavic1BeforeRoot = new EventNode
            {
                TextBlock = new EventTextBlock
                {
                    Fragments = new[]
                    {
                        new EventTextFragment
                        {
                            Speaker = UnitName.Environment, TextSid = "MainSlavic3Before_01_Text"
                        }
                    }
                },
                Options = new[]
                {
                    new EventOption("MainSlavic3Before_01_Option_01", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic3Before_02"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic3Before_02_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddStoryPointOptionAftermath("1")
                            }
                        }
                    }),
                    new EventOption("MainSlavic3Before_01_Option_02", new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = new[]
                            {
                                new EventTextFragment
                                {
                                    Speaker = UnitName.Environment, TextSid = "MainSlavic3Before_03"
                                }
                            }
                        },
                        Options = new[]
                        {
                            new EventOption("MainSlavic3Before_03_Option_01", EventNode.EndNode)
                            {
                                Aftermath = new AddStoryPointOptionAftermath("1")
                            }
                        }
                    })
                }
            };

            var mainSlavic1BeforeDialogue = new Dialogue(mainSlavic1BeforeRoot, EventPosition.BeforeCombat);
            return mainSlavic1BeforeDialogue;
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

            var dialogue = LoadDialogueFromResources(sid);

            return dialogue;
        }

        private static Dialogue LoadDialogueFromResources(string dialogueSid)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_PATH = "Rpg.Client.Resources.Dialogues";

            var dialogueSourcePath = RESOURCE_PATH + "." + dialogueSid + ".json";

            using var stream = assembly.GetManifestResourceStream(dialogueSourcePath);

            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();

                var deserializedLines = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                var deserializedDialogueNodes = deserializedLines.Where(x => x.Key != "root" && x.Key != "__editor").ToArray();

                // Fill node list

                var nodeList = new List<(string sid, IList<EventTextFragment> fragmentList, EventNode node)>();

                foreach (var (key, obj) in deserializedDialogueNodes)
                {
                    var dialogueTextFragments = new List<EventTextFragment>();
                    var dialogueNode = new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = dialogueTextFragments
                        }
                    };

                    var jsonSpeaker = obj.GetProperty("name").GetString();

                    var fragment = new EventTextFragment
                    {
                        Speaker = Enum.Parse<UnitName>(jsonSpeaker),
                        TextSid = $"{dialogueSid}_TextNode_{key}"
                    };

                    dialogueTextFragments.Add(fragment);

                    nodeList.Add(new(key, dialogueTextFragments, dialogueNode));
                }

                // Link nodes with options

                foreach (var (key, obj) in deserializedDialogueNodes)
                {
                    var nodeData = nodeList.Single(x => x.sid == key);

                    if (obj.TryGetProperty("choices", out var choices))
                    {
                        var optionIndex = 0;
                        var optionList = new List<EventOption>();
                        foreach (var choice in choices.EnumerateArray())
                        {
                            var nextId = choice.GetProperty("next").GetString();
                            var nextNodes = nodeList.Where(x => x.sid == nextId);
                            var nextNode = nextNodes.Any() ? nextNodes.Single().node : EventNode.EndNode;
                            var option = new EventOption($"{dialogueSid}_TextNode_{key}_Option_{optionIndex}", nextNode);

                            optionList.Add(option);

                            optionIndex += 1;
                        }

                        nodeData.node.Options = optionList;
                    }
                }

                var rootId = deserializedLines.Single(x => x.Key == "root").Value.GetProperty("next").GetString();

                var rootNode = nodeList.Single(x => x.sid == rootId).node;

                var position = dialogueSid.Contains("Before") ? EventPosition.BeforeCombat : EventPosition.AfterCombat;

                var dialogue = new Dialogue(rootNode, position);

                return dialogue;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Init()
        {
            var events = new List<Event>();

            Events = events;

            var mainPlot1 = new Event
            {
                Sid = "SlavicMain1",
                IsGameStart = true,
                IsUnique = true,
                BeforeCombatStartNodeSid = "SlavicMain1_Before"
            };

            events.Add(mainPlot1);

            var mainPlot2 = new Event
            {
                Sid = "MainSlavic2",
                IsUnique = true,
                BeforeCombatStartNodeSid = "MainSlavic2Before",
                Priority = TextEventPriority.High,
                Requirements = new[]
                {
                    new LocationEventRequirement(new[]
                    {
                        GlobeNodeSid.Battleground
                    })
                }
            };

            events.Add(mainPlot2);

            var mainPlot3 = new Event
            {
                Sid = "MainSlavic3",
                IsUnique = true,
                BeforeCombatStartNodeSid = "MainSlavic3Before",
                AfterCombatStartNodeSid = "MainSlavic3After",
                Priority = TextEventPriority.High,
                Requirements = new[]
                {
                    new LocationEventRequirement(new[]
                    {
                        GlobeNodeSid.Battleground
                    })
                }
            };

            events.Add(mainPlot3);

            var mainSlavic1BeforeDialogue = CreateMainSlavic1BeforeDialogue();
            var mainSlavic2BeforeDialogue = CreateMainSlavic2BeforeDialogue();
            var mainSlavic3BeforeDialogue = CreateMainSlavic3BeforeDialogue();
            var mainSlavic3AfterDialogue = CreateMainSlavic3AfterDialogue();

            _nodes = new Dictionary<string, Dialogue>
            {
                { "MainSlavic1Before", mainSlavic1BeforeDialogue },
                { "MainSlavic2Before", mainSlavic2BeforeDialogue },
                { "MainSlavic3Before", mainSlavic3BeforeDialogue },
                { "MainSlavic3After", mainSlavic3AfterDialogue }
            };

            _isInitialized = true;
        }
    }
}