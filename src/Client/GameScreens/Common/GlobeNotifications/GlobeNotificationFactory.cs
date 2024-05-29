using System;

using Client.Engine;

using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class GlobeNotificationFactory
{
    private readonly IUiContentStorage _uiContentStorage;

    public GlobeNotificationFactory(IUiContentStorage uiContentStorage)
    {
        _uiContentStorage = uiContentStorage;
    }

    public IGlobeNotification Create(Func<SpriteFont, IGlobeNotification> factoryDelegate)
    {
        return factoryDelegate(_uiContentStorage.GetMainFont());
    }
}
