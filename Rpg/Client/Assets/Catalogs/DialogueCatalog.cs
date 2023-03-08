using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.DialogueEventEnviroment;
using Client.Assets.DialogueOptionAftermath;
using Client.Assets.Dialogues;
using Client.Core.Dialogues;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Client.Assets.Catalogs;

internal class DialogueCatalog : IEventCatalog, IEventInitializer
{
    private readonly IDialogueEnvironmentEffectCreator _envCommandCreator;
    private readonly IDialogueOptionAftermathCreator _optionAftermathCreator;
    private readonly IDialogueResourceProvider _resourceProvider;

    private bool _isInitialized;

    public DialogueCatalog(IDialogueResourceProvider resourceProvider,
        IDialogueOptionAftermathCreator optionAftermathCreator)
    {
        _resourceProvider = resourceProvider;
        _optionAftermathCreator = optionAftermathCreator;

        _envCommandCreator = new DialogueEnvironmentEffectCreator();

        _isInitialized = false;
        Events = Array.Empty<DialogueEvent>();
    }

    private Dialogue LoadDialogue(string dialogueSid)
    {
        var dialogueYaml = _resourceProvider.GetResource(dialogueSid);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var dialogueDtoDict = deserializer.Deserialize<Dictionary<string, DialogueDtoScene>>(dialogueYaml);

        var services = new DialogueCatalogCreationServices(_envCommandCreator, _optionAftermathCreator);

        var dialogue = DialogueCatalogHelper.Create(dialogueSid, dialogueDtoDict, services);

        return dialogue;
    }

    public IEnumerable<DialogueEvent> Events { get; private set; }

    public Dialogue GetDialogue(string sid)
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException();
        }

        var dialogue = LoadDialogue(sid);

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