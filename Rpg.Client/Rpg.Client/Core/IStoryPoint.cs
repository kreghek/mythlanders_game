using System;

namespace Rpg.Client.Core
{
    public interface IStoryPoint : IJobExecutable
    {
        string Sid { get; }
        void Draw(IStoryPointDrawingContext context);

        event EventHandler? Completed;
    }
}