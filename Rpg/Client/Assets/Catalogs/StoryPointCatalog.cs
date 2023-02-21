using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Dialogues;
using Client.Core;
using Rpg.Client.Core;

namespace Client.Assets.Catalogs;

internal sealed class StoryPointCatalog : IStoryPointCatalog, IStoryPointInitializer
{
    private readonly IEventCatalog _eventCatalog;

    private IReadOnlyCollection<IStoryPoint> _storyPoints = new List<IStoryPoint>();
    public StoryPointCatalog(IEventCatalog eventCatalog)
    {
        _eventCatalog = eventCatalog;
    }

    public IReadOnlyCollection<IStoryPoint> GetAll()
    {
        return _storyPoints;
    }

    public void Init(Globe globe)
    {
        var spList = new List<IStoryPoint>();

        var dialogueFactoryType = typeof(IDialogueEventFactory);
        var factoryTypes = dialogueFactoryType.Assembly.GetTypes().Where(x =>
            dialogueFactoryType.IsAssignableFrom(x) && x != dialogueFactoryType && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance).OfType<IDialogueEventFactory>();

        var factoryServices = new DialogueEventFactoryServices(_eventCatalog);
            
        foreach (var factory in factories)
        {
            var storyPoints = factory.CreateStoryPoints(factoryServices);
            foreach (var storyPoint in storyPoints)
            {
                spList.Add(storyPoint);
            }
        }

        _storyPoints = spList;
    }
}