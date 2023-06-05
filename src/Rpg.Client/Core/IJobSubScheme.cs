namespace Rpg.Client.Core
{
    public interface IJobSubScheme
    {
        IJobScope Scope { get; }
        IJobType Type { get; }
        int Value { get; }
    }
}