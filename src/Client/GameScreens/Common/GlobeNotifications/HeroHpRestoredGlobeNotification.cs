using System;

using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class HeroHpRestoredGlobeNotification: GlobeNotificationBase
{
    private string _classSid;
    private readonly int _amount;
    private Texture2D _characterThumbnailIcons;

    public HeroHpRestoredGlobeNotification(string classSid, int amount, Texture2D characterThumbnailIcons)
    {
        _classSid = classSid;
        _amount = amount;
        _characterThumbnailIcons = characterThumbnailIcons;
    }

    protected override TextureRegion2D GetIcon()
    {
        return new TextureRegion2D(_characterThumbnailIcons, new Rectangle(0, 0, 32, 32));
    }

    protected override string GetNotificationMainRichText()
    {
        return GameObjectHelper.GetLocalized(Enum.Parse<UnitName>(_classSid)) + " HP +" + _amount;
    }

    protected override string GetNotificationTypeText()
    {
        return Client.UiResource.HeroRestoredNotificationText;
    }
}