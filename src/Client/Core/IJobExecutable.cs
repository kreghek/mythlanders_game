using System.Collections.Generic;

namespace Client.Core;

/// <summary>
/// Interface for entities, the completion of which depends on jobs (perks, quests, globe events).
/// </summary>
public interface IJobExecutable
{
    IReadOnlyCollection<IJob>? CurrentJobs { get; }
    bool IsComplete { get; }

    void HandleCompletion();
}