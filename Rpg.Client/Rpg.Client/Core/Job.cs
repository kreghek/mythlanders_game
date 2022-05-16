namespace Rpg.Client.Core
{
    internal sealed class Job: IJob
    {
        public bool IsComplete { get; set; }
        public int Progress { get; set; }
        public IJobSubScheme Scheme { get; set; }
    }
}