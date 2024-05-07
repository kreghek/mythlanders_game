namespace Client.Engine;

internal interface IUpdatableAnimationBlocker : IAnimationBlocker
{
    void Update(double elapsedSeconds);
}