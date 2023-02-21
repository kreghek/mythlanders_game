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

    public override string ToString()
    {
        var rm = UiResource.ResourceManager!;
        if (IsComplete)
        {
            return string.Format(rm.GetString(_completePatternResourceSid), _titleResourceSid);
        }

        return string.Format(rm.GetString(_patternResourceSid), rm.GetString(_titleResourceSid), Progress,
            Scheme.GoalValue.Value);
    }

    public bool IsComplete { get; set; }
    public int Progress { get; set; }
    public IJobSubScheme Scheme { get; }
}