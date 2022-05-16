namespace Rpg.Client.Core
{
    public interface IStoryPoint: IJobExecutable
    {
        bool IsComplete { get; }

        void Draw(IStoryPointDrawingContext context);
    }
}