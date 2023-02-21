using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Dialogues;
using Client.Assets.StoryPointAftermaths;
using Client.Core.Dialogues;

using Rpg.Client.Assets.StoryPointJobs;
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

    public IReadOnlyCollection<IStoryPoint> Init(Globe globe)
    {
        var spList = new List<IStoryPoint>();

        var activeList = new List<IStoryPoint>();

        var story2 = new StoryPoint("2")
        {
            TitleSid = "История2",
            CurrentJobs = new[]
            {
                new Job("Победа над противниками", "{0}: {1}/{2}", "{0} - завершено")
                {
                    Scheme = new JobScheme
                    {
                        Scope = JobScopeCatalog.Global,
                        Type = JobTypeCatalog.Defeats,
                        Value = 20
                    }
                }
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.Swamp)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.GreatWall)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.ScreamValley)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.Oasis))
            }
        };

        spList.Add(story2);

        var story1 = new StoryPoint("1")
        {
            TitleSid = "История1",
            CurrentJobs = new[]
            {
                new Job("Бои", "{0}: {1}/{2}", "{0} - завершено")
                {
                    Scheme = new JobScheme
                    {
                        Scope = JobScopeCatalog.Global,
                        Type = JobTypeCatalog.Combats,
                        Value = 6
                    }
                }
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.Battleground)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.GiantBamboo)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.Obelisk)),
                //new UnlockLocationAftermath(globe.Biomes.SelectMany(x => x.Nodes)
                //    .Single(x => x.Sid == GlobeNodeSid.Vines)),
                //new AddActivateStoryPointAftermath(story2, globe)
            }
        };

        activeList.Add(story1);
        spList.Add(story1);

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

        return activeList;
    }
}