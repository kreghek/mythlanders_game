using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.StoryPoints;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class StoryPointCatalog : IStoryPointCatalog, IStoryPointInitializer
    {
        private IReadOnlyCollection<IStoryPoint> _storyPoints = new List<IStoryPoint>();

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
                            Value = 10
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == GlobeNodeSid.Swamp)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes)
                        .Single(x => x.Sid == GlobeNodeSid.GreatWall)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes)
                        .Single(x => x.Sid == GlobeNodeSid.ScreamValley)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == GlobeNodeSid.Oasis))
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
                            Value = 3
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes)
                        .Single(x => x.Sid == GlobeNodeSid.Battleground)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes)
                        .Single(x => x.Sid == GlobeNodeSid.GiantBamboo)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes)
                        .Single(x => x.Sid == GlobeNodeSid.Obelisk)),
                    new UnlockLocation(globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == GlobeNodeSid.Vines)),
                    new AddActivateStoryPoint(story2, globe)
                }
            };

            activeList.Add(story1);
            spList.Add(story1);

            _storyPoints = spList;

            return activeList;
        }
    }

    internal sealed class DemoStoryPointCatalog : IStoryPointCatalog, IStoryPointInitializer
    {
        private IReadOnlyCollection<IStoryPoint> _storyPoints = new List<IStoryPoint>();

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
                            Value = 10
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
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
                            Value = 3
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
                    new AddActivateStoryPoint(story2, globe)
                }
            };

            activeList.Add(story1);
            spList.Add(story1);

            _storyPoints = spList;

            return activeList;
        }
    }
}