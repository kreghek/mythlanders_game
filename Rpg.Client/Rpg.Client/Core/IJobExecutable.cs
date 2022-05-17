using System.Collections.Generic;

namespace Rpg.Client.Core
{
    /// <summary>
    /// Интерфейс для сущностей, выполнение которых зависит от работ (перки, квесты).
    /// </summary>
    public interface IJobExecutable
    {
        bool IsComplete { get; }
        IReadOnlyCollection<IJob>? CurrentJobs { get; }

        void HandleCompletion();
    }
}