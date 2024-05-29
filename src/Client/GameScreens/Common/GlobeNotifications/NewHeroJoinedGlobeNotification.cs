using System;

using Client.Core;

using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class NewHeroJoinedGlobeNotification : GlobeNotificationBase
{
    private readonly string _classSid;

    public NewHeroJoinedGlobeNotification(string classSid, SpriteFont font) : base(font)
    {
        _classSid = classSid;
    }

    protected override string GetText()
    {
        return GameObjectHelper.GetLocalized(Enum.Parse<UnitName>(_classSid));
    }
}
