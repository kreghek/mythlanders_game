namespace Rpg.Client.Core
{
    internal sealed class Job : IJob
    {
        private readonly string _completePattern;
        private readonly string _pattern;

        public Job(string title, string pattern, string completePattern)
        {
            Title = title;
            _pattern = pattern;
            _completePattern = completePattern;
        }

        public string Title { get; }

        public override string ToString()
        {
            if (IsComplete)
            {
                return string.Format(_completePattern, Title);
            }

            return string.Format(_pattern, Title, Progress, Scheme.Value);
        }

        public bool IsComplete { get; set; }
        public int Progress { get; set; }
        public IJobSubScheme Scheme { get; set; } = null!;
    }
}