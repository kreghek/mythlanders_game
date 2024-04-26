using System;

namespace Client.Core;

public interface IStoryPoint : IDisplayableJobExecutable
{
    string Sid { get; }

    void Draw(IStoryPointDrawingContext context);

    event EventHandler? Completed;
}