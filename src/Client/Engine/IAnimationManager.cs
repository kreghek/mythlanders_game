namespace Client.Engine;

internal interface IAnimationManager
{
    bool HasBlockers { get; }
    void DropBlockers();
    void RegisterBlocker(IAnimationBlocker blocker);
}

internal static class IAnimationManagerExtensions
{
    public static IAnimationBlocker CreateAndRegisterBlocker(this IAnimationManager animationManager)
    {
        var blocker = new AnimationBlocker();

        animationManager.RegisterBlocker(blocker);

        return blocker;
    }
}