using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Rpg.Client.Assets.DialogueOptionAftermath;
using Rpg.Client.Assets.Dialogues;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal class DialogueCatalog : IEventCatalog, IEventInitializer
    {
        private readonly IDialogueOptionAftermathCreator _optionAftermathCreator;
        private readonly IDialogueResourceProvider _resourceProvider;

        private bool _isInitialized;

        public DialogueCatalog(IDialogueResourceProvider resourceProvider,
            IDialogueOptionAftermathCreator optionAftermathCreator)
        {
            _resourceProvider = resourceProvider;
            _optionAftermathCreator = optionAftermathCreator;

            _isInitialized = false;
            Events = Array.Empty<Event>();
        }

        private static EventTextFragment CreateEventTextFragment(string dialogueSid, JsonElement obj, string? key)
        {
            var jsonSpeaker = obj.GetProperty("name").GetString();

            var fragment = new EventTextFragment
            {
                Speaker = Enum.Parse<UnitName>(jsonSpeaker, ignoreCase: true), TextSid = $"{dialogueSid}_TextNode_{key}"
            };
            return fragment;
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
                        var parentNodeIdFromMap = textFragmentMergeMap[parentNodeId];

                        var nodeData = nodeList.Single(x => x.sid == parentNodeIdFromMap);
                        var fragmentList = nodeData.fragmentList;

                        var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                        fragmentList.Insert(0, fragment);

                        textFragmentMergeMap.Add(key, parentNodeIdFromMap);
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

            // Link nodes with options

            foreach (var (key, obj) in deserializedDialogueNodes)
            {
                if (nodeList.All(x => x.sid != key))
                {
                    // Means the json node was merged with other node as the text fragment.
                    continue;
                }

                var (sid, fragmentList, node) = nodeList.Single(x => x.sid == key);

                if (obj.TryGetProperty("choices", out var choices))
                {
                    var optionIndex = 0;
                    var optionList = new List<EventOption>();
                    foreach (var choice in choices.EnumerateArray())
                    {
                        EventNode nextNode;
                        IOptionAftermath? aftermath = null;

                        if (choice.TryGetProperty("next", out var choiceNext) &&
                            !string.IsNullOrEmpty(choiceNext.GetString()))
                        {
                            var nextId = choiceNext.GetString();

                            var mappedNextId = textFragmentMergeMap[nextId];

                            var nextNodes = nodeList.Where(x => x.sid == mappedNextId);
                            nextNode = nextNodes.Single().node;

                            var nextJsons = deserializedDialogueNodes.Where(x => x.Key == nextId);
                            if (nextJsons.Any())
                            {
                                var nextJson = nextJsons.Single();

                                if (nextJson.Value.TryGetProperty("signals", out var signals))
                                {
                                    var aftermathList = new List<IOptionAftermath>();
                                    foreach (var signalProperty in signals.EnumerateObject())
                                    {
                                        const string AFTERMATH_PREFIX = "AM_";
                                        if (signalProperty.Name.StartsWith(AFTERMATH_PREFIX))
                                        {
                                            var aftermathTypeName =
                                                signalProperty.Name.Substring(AFTERMATH_PREFIX.Length);
                                            if (aftermathTypeName.Contains("_"))
                                            {
                                                var postfixPosition = aftermathTypeName.LastIndexOf("_");
                                                aftermathTypeName = aftermathTypeName.Substring(0, postfixPosition);
                                            }

                                            var signalStringData =
                                                signalProperty.Value.GetProperty("String").GetString();

                                            if (signalStringData is not null)
                                            {
                                                var aftermathItem = _optionAftermathCreator.Create(aftermathTypeName,
                                                    signalStringData);

                                                aftermathList.Add(aftermathItem);
                                            }
                                            else
                                            {
                                                throw new InvalidOperationException("Data is not defined");
                                            }
                                        }
                                    }

                                    if (aftermathList.Any())
                                    {
                                        aftermath = new CompositeOptionAftermath(aftermathList);
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

                    node.Options = optionList;
                }
            }

            var rootId = deserializedLines.Single(x => x.Key == "root").Value.GetProperty("next").GetString();

            var rootNodeIdFromMap = textFragmentMergeMap[rootId];

            var rootNode = nodeList.Single(x => x.sid == rootNodeIdFromMap).node;

            var position = dialogueSid.Contains("Before") ? EventPosition.BeforeCombat : EventPosition.AfterCombat;

            var dialogue = new Dialogue(rootNode, position);

            return dialogue;
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

        public void Init()
        {
            var events = new List<Event>();

            Events = events;

            var dialogueFactoryType = typeof(IDialogueFactory);
            var factoryTypes = dialogueFactoryType.Assembly.GetTypes().Where(x =>
                dialogueFactoryType.IsAssignableFrom(x) && x != dialogueFactoryType && !x.IsAbstract);
            var factories = factoryTypes.Select(x => Activator.CreateInstance(x)).OfType<IDialogueFactory>();
            foreach (var factory in factories)
            {
                var dialogueEvent = factory.Create(this);
                events.Add(dialogueEvent);
            }

            _isInitialized = true;
        }
    }
}