using System;

using Rpg.Client.Core;

namespace Client.Core
{
    public interface IStoryPoint : IJobExecutable
    {
        string Sid { get; }
        string? TitleSid { get; init; }

        void Draw(IStoryPointDrawingContext context);

        event EventHandler? Completed;
    }
}