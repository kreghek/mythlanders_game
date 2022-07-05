namespace Rpg.Client.ScreenManagement
{
    internal interface IScreenManager
    {
        void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition,
            IScreenTransitionArguments args);
    }
}