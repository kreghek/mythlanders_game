using System;

namespace Client.Core;

public interface IChallenge : IJobExecutable
{
    event EventHandler? Completed;
}