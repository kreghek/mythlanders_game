namespace Client.Core;

public interface IJobSubScheme
{
    JobGoalValue GoalValue { get; }
    IJobScope Scope { get; }
    IJobType Type { get; }
}