using System;
using System.Collections.Generic;

using Core.Props;

namespace Client.Core;

internal sealed class CampaignChallenge : IChallenge
{
    private readonly Player _player;

    public CampaignChallenge(IReadOnlyCollection<IJob>? currentJobs, Player player)
    {
        _player = player;
        CurrentJobs = currentJobs;
    }

    public bool IsComplete { get; private set; }
    
    public IReadOnlyCollection<IJob>? CurrentJobs { get; }

    public void HandleCompletion()
    {
        IsComplete = true;
        Completed?.Invoke(this, EventArgs.Empty);
        
        _player.Inventory.Add(new Resource(new PropScheme("challenge"), 1));
    }

    public event EventHandler? Completed;
}