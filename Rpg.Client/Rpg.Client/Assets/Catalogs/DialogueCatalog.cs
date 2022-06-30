using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Assets.DialogueOptionAftermath;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal class DialogueCatalog : IEventCatalog, IEventInitializer
    {
        private readonly IDialogueResourceProvider _resourceProvider;
        private readonly IDialogueOptionAftermathCreator _optionAftermathCreator;

        private bool _isInitialized;

        public DialogueCatalog(IDialogueResourceProvider resourceProvider, IDialogueOptionAftermathCreator optionAftermathCreator)
        {
            _resourceProvider = resourceProvider;
            _optionAftermathCreator = optionAftermathCreator;

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
                                // Aftermath = new AddHeroOptionAftermath(
                                //     _unitSchemeCatalog.Heroes[UnitName.Archer])
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
                                // Aftermath = new AddHeroOptionAftermath(
                                //     _unitSchemeCatalog.Heroes[UnitName.Archer])
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

            var dialogue = LoadDialogueFromResources(sid);

            return dialogue;
        }

        private Dialogue LoadDialogueFromResources(string dialogueSid)
        {
            var json = _resourceProvider.GetResource(dialogueSid);

            var deserializedLines = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            var deserializedDialogueNodes =
                deserializedLines.Where(x => x.Key != "root" && x.Key != "__editor").ToArray();

            // Fill node list

            var nodeList = new List<(string sid, IList<EventTextFragment> fragmentList, EventNode node)>();

            var deserializedDialogueNodesOpenList = deserializedDialogueNodes.ToList();

            var textFragmentMergeMap = new Dictionary<string, string>();

            while (deserializedDialogueNodesOpenList.Any())
            {
                var (key, obj) = deserializedDialogueNodesOpenList.First();
                deserializedDialogueNodesOpenList.RemoveAt(0);

                if (obj.TryGetProperty("next", out var nextFragmentJson))
                {
                    var parentNodeId = nextFragmentJson.GetString();

                    if (deserializedDialogueNodesOpenList.Any(x => x.Key == parentNodeId))
                    {
                        // parent is not processed yet.
                        // Move current item to the end of the open list to process it later.

                        deserializedDialogueNodesOpenList.Add(new(key, obj));
                    }
                    else
                    {
                        var nodeData = nodeList.Single(x => x.sid == parentNodeId);
                        var fragmentList = nodeData.fragmentList;

                        var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                        fragmentList.Insert(0, fragment);

                        textFragmentMergeMap.Add(key, parentNodeId);
                    }
                }
                else
                {
                    var dialogueTextFragments = new List<EventTextFragment>();
                    var dialogueNode = new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = dialogueTextFragments
                        }
                    };

                    var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                    dialogueTextFragments.Add(fragment);

                    nodeList.Add(new(key, dialogueTextFragments, dialogueNode));

                    textFragmentMergeMap.Add(key, key);
                }
            }

            /*
            foreach (var (key, obj) in deserializedDialogueNodes)
            {
                if (obj.TryGetProperty("next", out var nextFragmentJson))
                {
                    var parentNodeId = nextFragmentJson.GetString();
                    var nodeData = nodeList.Single(x => x.sid == parentNodeId);
                    var fragmentList = nodeData.fragmentList;
                    
                    var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                    fragmentList.Add(fragment);
                }
                else
                {
                    var dialogueTextFragments = new List<EventTextFragment>();
                    var dialogueNode = new EventNode
                    {
                        TextBlock = new EventTextBlock
                        {
                            Fragments = dialogueTextFragments
                        }
                    };

                    var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                    dialogueTextFragments.Add(fragment);

                    nodeList.Add(new(key, dialogueTextFragments, dialogueNode));
                }
            }
            */

            // Link nodes with options

            foreach (var (key, obj) in deserializedDialogueNodes)
            {
                if (nodeList.All(x => x.sid != key))
                {
                    // Means the json node was merged with other node as the text fragment.
                    continue;
                }

                var nodeData = nodeList.Single(x => x.sid == key);

                if (obj.TryGetProperty("choices", out var choices))
                {
                    var optionIndex = 0;
                    var optionList = new List<EventOption>();
                    foreach (var choice in choices.EnumerateArray())
                    {
                        EventNode nextNode;
                        IOptionAftermath? aftermath = null;

                        if (choice.TryGetProperty("next", out var choiceNext))
                        {
                            var nextId = choiceNext.GetString();
                            var nextNodes = nodeList.Where(x => x.sid == nextId);
                            nextNode = nextNodes.Any() ? nextNodes.Single().node : EventNode.EndNode;

                            var nextJsons = deserializedDialogueNodes.Where(x => x.Key == nextId);
                            if (nextJsons.Any())
                            {
                                var nextJson = nextJsons.Single();

                                if (nextJson.Value.TryGetProperty("signals", out var signals))
                                {
                                    foreach (var signalProperty in signals.EnumerateObject())
                                    {
                                        const string AFTERMATH_PREFIX = "AM_";
                                        if (signalProperty.Name.StartsWith(AFTERMATH_PREFIX))
                                        {
                                            var aftermathTypeName = signalProperty.Name.Substring(AFTERMATH_PREFIX.Length);
                                            var signalStringData = signalProperty.Value.GetProperty("String").GetString();

                                            if (signalStringData is not null)
                                            {
                                                aftermath = _optionAftermathCreator.Create(aftermathTypeName,
                                                    signalStringData);
                                            }
                                            else
                                            {
                                                throw new InvalidOperationException("Data is not defined");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            nextNode = EventNode.EndNode;
                        }

                        var option = new EventOption($"{dialogueSid}_TextNode_{key}_Option_{optionIndex}", nextNode)
                        {
                            Aftermath = aftermath
                        };

                        optionList.Add(option);

                        optionIndex += 1;
                    }

                    nodeData.node.Options = optionList;
                }
            }

            var rootId = deserializedLines.Single(x => x.Key == "root").Value.GetProperty("next").GetString();

            var rootNodeIdFromMap = textFragmentMergeMap[rootId];

            var rootNode = nodeList.Single(x => x.sid == rootNodeIdFromMap).node;

            var position = dialogueSid.Contains("Before") ? EventPosition.BeforeCombat : EventPosition.AfterCombat;

            var dialogue = new Dialogue(rootNode, position);

            return dialogue;
        }

        private static EventTextFragment CreateEventTextFragment(string dialogueSid, JsonElement obj, string? key)
        {
            var jsonSpeaker = obj.GetProperty("name").GetString();

            var fragment = new EventTextFragment
            {
                Speaker = Enum.Parse<UnitName>(jsonSpeaker), TextSid = $"{dialogueSid}_TextNode_{key}"
            };
            return fragment;
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

            /*
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
            */

            _isInitialized = true;
        }
    }
}