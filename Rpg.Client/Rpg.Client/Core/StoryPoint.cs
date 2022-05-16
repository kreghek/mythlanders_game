using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Rpg.Client.Core
{
    internal sealed class StoryPoint: IStoryPoint
    {
        private readonly IStoryPointAftermathContext _aftermathContext;

        public StoryPoint(IStoryPointAftermathContext aftermathContext)
        {
            _aftermathContext = aftermathContext;
        }

        public IReadOnlyCollection<IJob>? CurrentJobs { get; init; }
        public void HandleCompletion()
        {
            if (Aftermaths is null)
            {
                return;
            }

            foreach (var storyPointAftermath in Aftermaths)
            {
                storyPointAftermath.Apply(_aftermathContext);
            }

            IsComplete = true;
        }

        public IReadOnlyCollection<IStoryPointAftermath>? Aftermaths { get; init; }
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
                    
                    concreteContext.TargetSpriteBatch.DrawString(concreteContext.StoryJobsFont, 
                        $"{currentJob.Scheme.Type}: {currentJob.Progress}/{currentJob.Scheme.Value}", 
                        (concreteContext.TargetRectangle.Location + new Point(0, index * JOB_HEIGHT)).ToVector2(),
                        Color.White);
                }

                jobsHeight = currentJobList.Length * JOB_HEIGHT;
            }
            
            concreteContext.ResultRectangle = new Rectangle(concreteContext.TargetRectangle.Location,
                new Point(concreteContext.TargetRectangle.Width, TITLE_HEIGHT + jobsHeight));
        }

        public string? TitleSid { get; init; }
    }

    internal sealed class ActivateStoryPoint: IStoryPointAftermath
    {
        private readonly IStoryPoint _newStoryPoint;
        private readonly Globe _globe;

        public ActivateStoryPoint(IStoryPoint newStoryPoint, Globe globe)
        {
            _newStoryPoint = newStoryPoint;
            _globe = globe;
        }

        public void Apply(IStoryPointAftermathContext context)
        {
            _globe.ActiveStoryPoints = _globe.ActiveStoryPoints.Concat(new[] { _newStoryPoint }).ToArray();
        }
    }
}