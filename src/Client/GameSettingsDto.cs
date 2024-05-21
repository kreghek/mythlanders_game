namespace Client;

internal sealed record GameSettingsDto(bool IsFullScreen, int ScreenWidth, int ScreenHeight, string Language,
    float Music);