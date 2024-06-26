﻿using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class MonsterPerkAddedGlobeNotification : GlobeNotificationBase
{
    private readonly Texture2D _perkIcons;
    private readonly MonsterPerk _targetPerk;

    public MonsterPerkAddedGlobeNotification(MonsterPerk targetPerk, Texture2D perkIcons)
    {
        _targetPerk = targetPerk;
        _perkIcons = perkIcons;
    }

    protected override TextureRegion2D GetIcon()
    {
        return new TextureRegion2D(_perkIcons, new Rectangle(0, 64, 64, 64));
    }

    protected override string GetNotificationMainRichText()
    {
        return GameObjectHelper.GetLocalizedMonsterPerk(_targetPerk.Sid);
    }

    protected override string GetNotificationTypeText()
    {
        return UiResource.MonsterPerkAddedNotificationText;
    }
}