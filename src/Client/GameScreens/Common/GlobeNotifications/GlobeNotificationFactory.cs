using System;

using Client.Engine;
using Client.GameScreens.Combat.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class GlobeNotificationFactory
{
    private readonly IUiContentStorage _uiContentStorage;
    private readonly ICombatantThumbnailProvider _combatantThumbnailProvider;
    private readonly Texture2D _monsterPerkTexture;

    public GlobeNotificationFactory(IUiContentStorage uiContentStorage, ICombatantThumbnailProvider combatantThumbnailProvider, Game game)
    {
        _uiContentStorage = uiContentStorage;
        _combatantThumbnailProvider = combatantThumbnailProvider;
        _monsterPerkTexture = game.Content.Load<Texture2D>("Sprites/GameObjects/MonsterPerkIcons");
    }

    public IGlobeNotification Create(Func<ICombatantThumbnailProvider, Texture2D, IGlobeNotification> factoryDelegate)
    {
        return factoryDelegate(_combatantThumbnailProvider, _monsterPerkTexture);
    }
}