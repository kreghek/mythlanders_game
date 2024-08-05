using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Engine.Ui;

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

            const int NOTIFICATION_WIDTH = 200;
            const int NOTIFICATION_HEIGHT = 32 + ControlBase.CONTENT_MARGIN * 8;

            var notificationContentRect = new Rectangle(contentRectangle.Center.X - NOTIFICATION_WIDTH / 2,
                contentRectangle.Top + NOTIFICATION_HEIGHT * index,
                NOTIFICATION_WIDTH,
                NOTIFICATION_HEIGHT);

            var t = (float)notification.Lifetime > 3 ? 1 : ((float)notification.Lifetime) / 3;
            notification.Notification.Draw(spriteBatch, t, notificationContentRect);
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