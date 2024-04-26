namespace Client.Core;

public interface IDisplayableJobExecutable : IJobExecutable
{
    string TitleSid { get; }
    int Order { get; }
}