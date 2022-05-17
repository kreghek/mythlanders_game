namespace Rpg.Client.Core
{
    internal sealed class Job : IJob
    {
        private readonly string _pattern;
        private readonly string _completePattern;

        public bool IsComplete { get; set; }
        public int Progress { get; set; }
        public IJobSubScheme Scheme { get; set; } = null!;

        public string Title { get; }

        public Job(string title, string pattern, string completePattern)
        {
            Title = title;
            _pattern = pattern;
            _completePattern = completePattern;
        }

        public override string ToString()
        {
            if (IsComplete)
            {
                return string.Format(_completePattern, Title);
            }

            return string.Format(_pattern, Title, Progress, Scheme.Value);
        }
    }
}