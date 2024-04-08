using Client.ScreenManagement;

namespace Client.GameScreens.Bestiary;

internal sealed record BestiaryScreenTransitionArguments
    (ScreenTransition ParentScreen, object ParentScreenArgs) : IScreenTransitionArguments;