using System;
using System.Collections.Generic;

namespace Client.Core;

internal sealed class CampaignChallenge : IChallenge
{
    public CampaignChallenge(IReadOnlyCollection<IJob>? currentJobs)
    {
        CurrentJobs = currentJobs;
    }

    public bool IsComplete { get; private set; }
    
    public IReadOnlyCollection<IJob>? CurrentJobs { get; }

    public void HandleCompletion()
    {
        IsComplete = true;
        Completed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? Completed;
}