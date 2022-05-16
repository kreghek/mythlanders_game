using System.Linq;

namespace Rpg.Client.Core
{
    public sealed class JobProgressResolver: IJobProgressResolver
    {
        public void ApplyProgress(IJobProgress progress, IJobExecutable target)
        {
            if (target.CurrentJobs is null)
            {
                // Перки у которых нет работ, не могут развиваться.

                // Некоторые перки (например врождённые таланты), не прокачиваются.
                // Сразу игнорируем их.
                return;
            }

            var affectedJobs = progress.ApplyToJobs(target.CurrentJobs);

            foreach (var job in affectedJobs)
            {
                // Опеределяем, какие из прогрессировавших работ завершены.
                // И фиксируем их состояние завершения.
                if (job.Progress >= job.Scheme.Value)
                {
                    job.IsComplete = true;
                }
            }
            
            // Опеределяем, все ли работы выполнены.
            var allJobsAreComplete = target.CurrentJobs.All(x => x.IsComplete);

            if (allJobsAreComplete)
            {
                target.HandleCompletion();
            }
        }
    }
}