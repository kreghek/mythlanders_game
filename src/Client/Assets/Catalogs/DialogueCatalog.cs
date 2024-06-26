﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Client.Assets.Catalogs;

internal class DialogueCatalog : IEventCatalog, IEventInitializer
{
    private readonly IDialogueConditionCreator<ParagraphConditionContext> _dialogueConditionCreator;
    private readonly IDialogueParagraphEffectCreator<CampaignAftermathContext> _envCommandCreator;
    private readonly IDialogueOptionAftermathCreator<CampaignAftermathContext> _optionAftermathCreator;
    private readonly IDialogueResourceProvider _resourceProvider;

    private bool _isInitialized;

    public DialogueCatalog(IDialogueResourceProvider resourceProvider,
        IDialogueOptionAftermathCreator<CampaignAftermathContext> optionAftermathCreator,
        IDialogueParagraphEffectCreator<CampaignAftermathContext> environmentEffectCreator,
        IDialogueConditionCreator<ParagraphConditionContext> dialogueConditionCreator)
    {
        _resourceProvider = resourceProvider;
        _optionAftermathCreator = optionAftermathCreator;

        _envCommandCreator = environmentEffectCreator;
        _dialogueConditionCreator = dialogueConditionCreator;

        _isInitialized = false;
        Events = Array.Empty<DialogueEvent>();
    }

    private Dialogue<ParagraphConditionContext, CampaignAftermathContext> LoadDialogue(string dialogueSid)
    {
        var dialogueYaml = _resourceProvider.GetResource(dialogueSid);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var dialogueDtoDict = deserializer.Deserialize<Dictionary<string, DialogueDtoScene>>(dialogueYaml);

        var services =
            new DialogueCatalogCreationServices<ParagraphConditionContext, CampaignAftermathContext>(
                _envCommandCreator,
                _optionAftermathCreator,
                _dialogueConditionCreator);

        var dialogue = DialogueCatalogHelper.Create(dialogueSid,
            dialogueDtoDict, services,
            (speaker => new[] { new HasHeroParagraphCondition(speaker) }));

        return dialogue;
    }

    public IEnumerable<DialogueEvent> Events { get; private set; }

    public Dialogue<ParagraphConditionContext, CampaignAftermathContext> GetDialogue(string sid)
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