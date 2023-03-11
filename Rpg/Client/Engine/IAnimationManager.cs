namespace Rpg.Client.Engine
{
    internal interface IAnimationManager
    {
        bool HasBlockers { get; }
        void RegisterBlocker(IAnimationBlocker blocker);
        void DropBlockers();
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
}