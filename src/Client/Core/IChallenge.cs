using System;

namespace Client.Core;

public interface IChallenge : IDisplayableJobExecutable
{
    event EventHandler? Completed;
}