using System;

namespace Rpg.Client.Core
{
    public interface IStoryPoint : IJobExecutable
    {
        void Draw(IStoryPointDrawingContext context);

        event EventHandler? Completed;
    }
}