namespace Client.Core;

internal sealed class JobScheme : IJobSubScheme
{
    public JobScheme(IJobScope scope, IJobType type, JobGoalValue goalValue)
    {
        Scope = scope;
        Type = type;
        GoalValue = goalValue;
    }

    public IJobScope Scope { get; }
    public IJobType Type { get; }
    public JobGoalValue GoalValue { get; }
}