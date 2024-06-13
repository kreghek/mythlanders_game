using System;

using Client.GameScreens.Combat.Ui;

using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal interface IGlobeNotificationFactory
{
    IGlobeNotification Create(Func<ICombatantThumbnailProvider, Texture2D, IGlobeNotification> factoryDelegate);
}