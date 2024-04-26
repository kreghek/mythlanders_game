namespace Client.Core;

public interface IDisplayableJobExecutable : IJobExecutable
{
    int Order { get; }
    string TitleSid { get; }
}