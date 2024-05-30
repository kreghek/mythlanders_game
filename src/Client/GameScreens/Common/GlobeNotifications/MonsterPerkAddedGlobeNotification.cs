using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class MonsterPerkAddedGlobeNotification : GlobeNotificationBase
{
    private readonly MonsterPerk _targetPerk;
    private readonly Texture2D _perkIcons;

    public MonsterPerkAddedGlobeNotification(MonsterPerk targetPerk, Texture2D perkIcons)
    {
        _targetPerk = targetPerk;
        _perkIcons = perkIcons;
    }

    protected override string GetNotificationMainRichText()
    {
        return GameObjectHelper.GetLocalizedMonsterPerk(_targetPerk.Sid);
    }

    protected override string GetNotificationTypeText()
    {
        return "Monster Perk Added";
    }

    protected override TextureRegion2D GetIcon()
    {
        return new TextureRegion2D(_perkIcons, new Rectangle(0, 0, 32, 32));
    }
}