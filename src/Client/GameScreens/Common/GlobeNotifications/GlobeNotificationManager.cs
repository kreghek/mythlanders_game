using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class GlobeNotificationManager : IGlobeNotificationManager
{
    private readonly IList<GlobeNotificationLifetime> _currentNotifications;

    public GlobeNotificationManager()
    {
        _currentNotifications = new List<GlobeNotificationLifetime>();
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        for (var index = 0; index < _currentNotifications.ToArray().Length; index++)
        {
            var notification = _currentNotifications[index];

            const int NOTIFICATION_WIDTH = 300;
            const int NOTIFICATION_HEIGHT = 30;

            var notificationContentRect = new Rectangle(contentRectangle.Center.X - NOTIFICATION_WIDTH / 2,
                contentRectangle.Top + NOTIFICATION_HEIGHT * index,
                NOTIFICATION_WIDTH,
                NOTIFICATION_HEIGHT);

            spriteBatch.Draw(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                notificationContentRect,
                new Rectangle(0, 0, 32, 32),
                Color.Lerp(Color.White, Color.Transparent, 1 - (float)notification.Lifetime));

            notification.Notification.Draw(spriteBatch, (float)notification.Lifetime, notificationContentRect);
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var notification in _currentNotifications.ToArray())
        {
            notification.Lifetime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (notification.Lifetime <= 0)
            {
                _currentNotifications.Remove(notification);
            }
        }
    }

    public void AddNotification(IGlobeNotification notification)
    {
        _currentNotifications.Add(new GlobeNotificationLifetime(notification));
    }

    private sealed class GlobeNotificationLifetime
    {
        public GlobeNotificationLifetime(IGlobeNotification notification)
        {
            Notification = notification;
            Lifetime = 5;
        }

        public double Lifetime { get; set; }
        public IGlobeNotification Notification { get; }
    }
}