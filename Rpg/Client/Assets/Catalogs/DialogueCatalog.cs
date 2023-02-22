using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Client.Assets.Catalogs;
using Client.Assets.DialogueEventEnviroment;
using Client.Assets.DialogueOptionAftermath;
using Client.Assets.Dialogues;
using Client.Core.Dialogues;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal class DialogueCatalog : IEventCatalog, IEventInitializer
    {
        private readonly IDialogueOptionAftermathCreator _optionAftermathCreator;
        private readonly IDialogueResourceProvider _resourceProvider;
        private readonly DialogueEnvCommandCreator _envCommandCreator;

        private bool _isInitialized;

        public DialogueCatalog(IDialogueResourceProvider resourceProvider,
            IDialogueOptionAftermathCreator optionAftermathCreator)
        {
            _resourceProvider = resourceProvider;
            _optionAftermathCreator = optionAftermathCreator;

            _envCommandCreator = new DialogueEnvCommandCreator();

            _isInitialized = false;
            Events = Array.Empty<DialogueEvent>();
        }

        private EventTextFragment CreateEventTextFragment(string dialogueSid, JsonElement obj, string? key)
        {
            var jsonSpeaker = obj.GetProperty("name").GetString();

            if (!Enum.TryParse<UnitName>(jsonSpeaker, ignoreCase: true, out var unitName))
            {
                unitName = UnitName.Environment;
            }

            var enviromentCommandList = new List<IDialogueEventTextFragmentEnvironmentCommand>();

            if (obj.TryGetProperty("signals", out var signals))
            {
                foreach (var signalProperty in signals.EnumerateObject())
                {
                    const string ENVIRONMENT_PREFFIX = "ENV_";
                    if (signalProperty.Name.StartsWith(ENVIRONMENT_PREFFIX))
                    {
                        var (envTypeName, envData) = Handle(signalProperty, ENVIRONMENT_PREFFIX);

                        var envCommand = _envCommandCreator.Create(envTypeName, envData);

                        enviromentCommandList.Add(envCommand);

                    }
                }
            }

            var fragment = new EventTextFragment(unitName, $"{dialogueSid}_TextNode_{key}")
            {
                EnvironmentCommands = enviromentCommandList
                //new IDialogueEventTextFragmentEnvironmentCommand[]
                //{
                //    new PlaySoundEnviromentCommand("desert_winds", "DesertWind")
                //}
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

            var nodeList =
                new List<(string sid, IList<EventTextFragment> fragmentList, DialogueNode node, List<DialogueOption>
                    options)>();

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

                    var textBlock = new EventTextBlock(dialogueTextFragments);

                    var dialogOptions = new List<DialogueOption>();

                    var dialogueNode = new DialogueNode(textBlock, dialogOptions);

                    var fragment = CreateEventTextFragment(dialogueSid: dialogueSid, obj: obj, key: key);

                    dialogueTextFragments.Add(fragment);

                    nodeList.Add(new(key, dialogueTextFragments, dialogueNode, dialogOptions));

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

                var (sid, fragmentList, node, dialogOptions) = nodeList.Single(x => x.sid == key);

                if (obj.TryGetProperty("choices", out var choices))
                {
                    var optionIndex = 0;
                    foreach (var choice in choices.EnumerateArray())
                    {
                        DialogueNode nextNode;
                        IDialogueOptionAftermath? aftermath = null;

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
                                    var aftermathList = new List<IDialogueOptionAftermath>();
                                    var enviromentList = new List<IDialogueEventTextFragmentEnvironmentCommand>();
                                    foreach (var signalProperty in signals.EnumerateObject())
                                    {
                                        const string AFTERMATH_PREFIX = "AM_";
                                        const string ENVIRONMENT_PREFFIX = "ENV_";
                                        if (signalProperty.Name.StartsWith(AFTERMATH_PREFIX))
                                        {
                                            var (aftermathTypeName, aftermathData) = Handle(signalProperty, AFTERMATH_PREFIX);
                                            var aftermathItem = _optionAftermathCreator.Create(aftermathTypeName, aftermathData);

                                            aftermathList.Add(aftermathItem);
                                        }
                                        else if (signalProperty.Name.StartsWith(ENVIRONMENT_PREFFIX))
                                        {
                                            var (envTypeName, envData) = Handle(signalProperty, ENVIRONMENT_PREFFIX);

                                            if (envTypeName == "PlaySound")
                                            {
                                                var command = new PlayEffectEnviromentCommand(envData, envData);
                                                enviromentList.Add(command);
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
                            nextNode = DialogueNode.EndNode;
                        }

                        var option = new DialogueOption($"{dialogueSid}_TextNode_{key}_Option_{optionIndex}", nextNode)
                        {
                            Aftermath = aftermath
                        };

                        dialogOptions.Add(option);

                        optionIndex += 1;
                    }
                }
            }

            var rootId = deserializedLines.Single(x => x.Key == "root").Value.GetProperty("next").GetString();

            var rootNodeIdFromMap = textFragmentMergeMap[rootId];

            var rootNode = nodeList.Single(x => x.sid == rootNodeIdFromMap).node;

            var dialogue = new Dialogue(rootNode);

            return dialogue;
        }

        private static (string typeName, string data) Handle(JsonProperty signalProperty, string preffix)
        {
            var aftermathTypeName = signalProperty.Name.Substring(preffix.Length);
            if (aftermathTypeName.Contains('_'))
            {
                var postfixPosition = aftermathTypeName.LastIndexOf("_");
                aftermathTypeName = aftermathTypeName.Substring(0, postfixPosition);
            }

            var signalStringData = signalProperty.Value.GetProperty("String").GetString();

            if (signalStringData is not null)
            {
                return (aftermathTypeName, signalStringData);
            }
            else
            {
                throw new InvalidOperationException("Data is not defined");
            }
        }

        public IEnumerable<DialogueEvent> Events { get; private set; }

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
            var events = new List<DialogueEvent>();

            Events = events;

            var dialogueFactoryType = typeof(IDialogueEventFactory);
            var factoryTypes = dialogueFactoryType.Assembly.GetTypes().Where(x =>
                dialogueFactoryType.IsAssignableFrom(x) && x != dialogueFactoryType && !x.IsAbstract);
            var factories = factoryTypes.Select(Activator.CreateInstance).OfType<IDialogueEventFactory>();

            var factoryServices = new DialogueEventFactoryServices(this);

            foreach (var factory in factories)
            {
                var dialogueEvent = factory.CreateEvent(factoryServices);
                events.Add(dialogueEvent);
            }

            _isInitialized = true;
        }
    }
}