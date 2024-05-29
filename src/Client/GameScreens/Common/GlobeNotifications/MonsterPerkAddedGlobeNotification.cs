using Client.Core;

using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class MonsterPerkAddedGlobeNotification : GlobeNotificationBase
{
    private readonly MonsterPerk _targetPerk;

    public MonsterPerkAddedGlobeNotification(MonsterPerk targetPerk, SpriteFont font) : base(font)
    {
        _targetPerk = targetPerk;
    }

    protected override string GetText()
    {
        return GameObjectHelper.GetLocalizedMonsterPerk(_targetPerk.Sid);
    }
}