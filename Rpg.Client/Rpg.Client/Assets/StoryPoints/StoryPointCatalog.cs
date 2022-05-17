using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPoints
{
    internal sealed class StoryPointCatalog
    {
        public IReadOnlyCollection<IStoryPoint> Create(Globe globe)
        {
            var activeList = new List<IStoryPoint>();

            var story2 = new StoryPoint
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
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Swamp)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.GreatWall)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.ScreamValey)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Oasis)),
                }
            };

            var story1 = new StoryPoint
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
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Battleground)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.GaintBamboo)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Obelisk)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Vines)),
                    new AddActivateStoryPoint(story2, globe)
                }
            };

            activeList.Add(story1);

            return activeList;
        }
    }
}