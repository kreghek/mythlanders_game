using System;

using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class NewHeroJoinedGlobeNotification : GlobeNotificationBase
{
    private readonly Texture2D _characterThumbnailIcons;
    private readonly string _classSid;

    public NewHeroJoinedGlobeNotification(string classSid, Texture2D characterThumbnailIcons)
    {
        _classSid = classSid;
        _characterThumbnailIcons = characterThumbnailIcons;
    }

    protected override TextureRegion2D GetIcon()
    {
        return new TextureRegion2D(_characterThumbnailIcons, new Rectangle(0, 0, 32, 32));
    }

    protected override string GetNotificationMainRichText()
    {
        return GameObjectHelper.GetLocalized(Enum.Parse<UnitName>(_classSid));
    }

    protected override string GetNotificationTypeText()
    {
        return UiResource.NewHeroJoinedNotificationText;
    }
}