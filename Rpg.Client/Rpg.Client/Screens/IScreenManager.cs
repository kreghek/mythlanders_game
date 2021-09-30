namespace Rpg.Client.Screens
{
    internal interface IScreenManager
    {
        void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition);
    }
}