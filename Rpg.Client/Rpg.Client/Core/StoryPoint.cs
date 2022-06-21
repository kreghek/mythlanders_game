using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal sealed class StoryPoint : IStoryPoint
    {
        public StoryPoint(string sid)
        {
            Sid = sid;
        }

        public IReadOnlyCollection<IStoryPointAftermath>? Aftermaths { get; init; }

        public string Sid { get; }
        public string? TitleSid { get; init; }
        public IReadOnlyCollection<IJob>? CurrentJobs { get; init; }

        public void HandleCompletion()
        {
            if (Aftermaths is null)
            {
                return;
            }

            foreach (var storyPointAftermath in Aftermaths)
            {
                storyPointAftermath.Apply();
            }

            IsComplete = true;
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public bool IsComplete { get; private set; }

        public void Draw(IStoryPointDrawingContext context)
        {
            var concreteContext = (StoryPointDrawingContext)context;

            const int TITLE_TEXT_HEIGHT = 20;
            const int TITLE_TEXT_MARGIN = 5;
            const int TITLE_HEIGHT = TITLE_TEXT_HEIGHT + TITLE_TEXT_MARGIN;

            concreteContext.TargetSpriteBatch.DrawString(concreteContext.StoryTitleFont,
                TitleSid,
                concreteContext.TargetRectangle.Location.ToVector2(),
                Color.White);

            var jobsHeight = 0;
            const int JOB_TEXT_HEIGHT = 20;
            const int JOB_TEXT_MARGIN = 5;
            const int JOB_HEIGHT = JOB_TEXT_HEIGHT + JOB_TEXT_MARGIN;
            if (CurrentJobs is not null)
            {
                var currentJobList = CurrentJobs.ToArray();
                for (var index = 0; index < currentJobList.Length; index++)
                {
                    var currentJob = currentJobList[index];

                    var jobTextPosition = concreteContext.TargetRectangle.Location +
                                          new Point(0, index * JOB_HEIGHT + TITLE_HEIGHT);
                    concreteContext.TargetSpriteBatch.DrawString(concreteContext.StoryJobsFont,
                        currentJob.ToString(),
                        jobTextPosition.ToVector2(),
                        Color.White);
                }

                jobsHeight = currentJobList.Length * JOB_HEIGHT;
            }

            concreteContext.ResultRectangle = new Rectangle(concreteContext.TargetRectangle.Location,
                new Point(concreteContext.TargetRectangle.Width, TITLE_HEIGHT + jobsHeight));
        }

        public event EventHandler? Completed;
    }
}