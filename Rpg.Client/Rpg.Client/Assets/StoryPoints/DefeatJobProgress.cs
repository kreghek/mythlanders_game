using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.StoryPoints
{
    internal sealed class DefeatJobProgress : IJobProgress
    {
        private static void ProcessJob(IJob job, ICollection<IJob> modifiedJobs)
        {
            job.Progress++;
            modifiedJobs.Add(job);
        }

        public IEnumerable<IJob> ApplyToJobs(IEnumerable<IJob> currentJobs)
        {
            if (currentJobs is null)
            {
                throw new ArgumentNullException(nameof(currentJobs));
            }

            var modifiedJobs = new List<IJob>();
            foreach (var job in currentJobs)
            {
                if (job.Scheme.Type != JobTypeCatalog.Defeats)
                {
                    continue;
                }

                ProcessJob(job, modifiedJobs);
            }

            return modifiedJobs.ToArray();
        }
    }
}