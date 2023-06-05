namespace Rpg.Client.Engine
{
    internal interface IAnimationManager
    {
        bool HasBlockers { get; }
        AnimationBlocker CreateAndUseBlocker();
        void DropBlockers();
    }
}