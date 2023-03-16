using Rpg.Client.Core;

namespace Client.Core;

internal sealed class Job : IJob
{
    private readonly string _completePatternResourceSid;
    private readonly string _patternResourceSid;

    private readonly string _titleResourceSid;

    public Job(
        IJobSubScheme scheme,
        string titleResourceSid,
        string patternResourceSid,
        string completePatternResourceSid)
    {
        _titleResourceSid = titleResourceSid;
        _patternResourceSid = patternResourceSid;
        _completePatternResourceSid = completePatternResourceSid;
        Scheme = scheme;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var rm = UiResource.ResourceManager;
        if (IsComplete)
        {
            var completePattern = rm.GetString(_completePatternResourceSid);
            if (completePattern is not null)
            {
                return string.Format(completePattern, _titleResourceSid);
            }
            else
            {
                return _completePatternResourceSid;
            }
        }

        var inProgressPattern = rm.GetString(_patternResourceSid);

        if (inProgressPattern is not null)
        {
            return string.Format(inProgressPattern, rm.GetString(_titleResourceSid), Progress,
                Scheme.GoalValue.Value);
        }
        else
        {
            return $"{_titleResourceSid} {Progress}/{Scheme.GoalValue.Value}";
        }
    }

    public bool IsComplete { get; set; }
    public int Progress { get; set; }
    public IJobSubScheme Scheme { get; }
}