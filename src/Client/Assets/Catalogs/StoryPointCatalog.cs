using System;
using System.Collections.Generic;
using System.Linq;

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

    public IReadOnlyCollection<IStoryPoint> GetAll()
    {
        return _storyPoints;
    }

    public void Init(Globe globe)
    {
        var spList = new List<IStoryPoint>();

        InitStoryPointsFromDialogues(spList);

        spList.Add(new StoryPoint("HearMeBrothersFeat1")
        {
            // Unlock dialogue to unlock desert, monastery and chtulhu + unlock large campaigns
            TitleSid = "HearMeBrothersFeat1",
            CurrentJobs = new[]
            {
                new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.Defeats, new JobGoalValue(10)),
                    "DefeatAnyEnemies", "CommonJobInProgressPattern", "CommonJobCompletePattern"),
                new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.WinCampaigns, new JobGoalValue(2)),
                    "WinCampaigns", "CommonJobInProgressPattern", "CommonJobCompletePattern")
            },
            Aftermaths = new[] { new AddStoryKeyStoryPointAftermath("HearMeBrothersFeat1Complete", globe) }
        });

        spList.Add(new StoryPoint("HearMeBrothersFeat2")
        {
            // Unlock sets of dialogues to join other start heroes
            TitleSid = "HearMeBrothersFeat2",
            CurrentJobs = new[]
            {
                new Job(new JobScheme(JobScopeCatalog.Campaign, JobTypeCatalog.Defeats, new JobGoalValue(30)),
                    "DefeatAnyEnemiesDuringCampaign", "CommonJobInProgressPattern", "CommonJobCompletePattern")
            },
            Aftermaths = new[] { new AddStoryKeyStoryPointAftermath("HearMeBrothersFeat2Complete", globe) }
        });

        spList.Add(new StoryPoint("HearMeBrothersFeat3")
        {
            // Unlock plot stage to meet with main evil + unlock monster perks
            TitleSid = "HearMeBrothersFeat3",
            CurrentJobs = new[]
            {
                new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.Defeats, new JobGoalValue(30)),
                    "DefeatAnyEnemiesDuringCampaign", "CommonJobInProgressPattern", "CommonJobCompletePattern")
            },
            Aftermaths = new[] { new AddStoryKeyStoryPointAftermath("HearMeBrothersFeat3Complete", globe) }
        });

        _storyPoints = spList;
    }
}