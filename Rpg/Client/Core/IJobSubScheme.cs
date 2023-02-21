using Rpg.Client.Core;

namespace Client.Core;

public interface IJobSubScheme
{
    IJobScope Scope { get; }
    IJobType Type { get; }
    JobGoalValue GoalValue { get; }
}