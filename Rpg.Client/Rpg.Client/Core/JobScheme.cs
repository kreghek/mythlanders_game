namespace Rpg.Client.Core
{
    internal sealed class JobScheme: IJobSubScheme
    {
        public IJobScope Scope { get; set; }
        public IJobType Type { get; set; }
        public int Value { get; set; }
    }
}