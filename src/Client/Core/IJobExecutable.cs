using System.Collections.Generic;

namespace Client.Core;

/// <summary>
/// Интерфейс для сущностей, выполнение которых зависит от работ (перки, квесты).
/// </summary>
public interface IJobExecutable
{
    IReadOnlyCollection<IJob>? CurrentJobs { get; }
    bool IsComplete { get; }

    void HandleCompletion();
}