using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.DialogueOptionAftermath;
using Client.Assets.Dialogues;
using Client.Assets.StoryPointAftermaths;
using Client.Assets.StoryPointJobs;
using Client.Core;

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

        InitStoryPointsFromDialogues(spList);

        spList.Add(new StoryPoint("HearMeBrothers")
        {
            CurrentJobs = new[]
            {
                new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.Defeats, new JobGoalValue(200)),
                    String.Empty, String.Empty, String.Empty),
                new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.CompleteCampaigns, new JobGoalValue(5)),
                    String.Empty, String.Empty, String.Empty),
                new Job(new JobScheme(JobScopeCatalog.Campaign, JobTypeCatalog.Defeats, new JobGoalValue(20)),
                    String.Empty, String.Empty, String.Empty)
            },
            Aftermaths = new[] { new AddStoryKeyStoryPointAftermath("HearMeBrothersComplete", globe) }
        });

        _storyPoints = spList;
    }

    private void InitStoryPointsFromDialogues(ICollection<IStoryPoint> spList)
    {
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
    }
}