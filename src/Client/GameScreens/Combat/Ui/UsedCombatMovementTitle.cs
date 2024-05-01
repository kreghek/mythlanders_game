using System;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

public class UsedCombatMovementTitle
{
    private const double COMBAT_MOVEMENT_TITLE_DURATION_SECONDS = 1.2;
    private readonly CombatMovementSid _combatMovementSid;

    private readonly SpriteFont _font;
    private readonly bool _isHeroSide;

    private double _usedCombatMovementCounterSeconds;

    public UsedCombatMovementTitle(SpriteFont font, CombatMovementSid combatMovementSid, bool isHeroSide)
    {
        _font = font;
        _combatMovementSid = combatMovementSid;
        _isHeroSide = isHeroSide;

        _usedCombatMovementCounterSeconds = COMBAT_MOVEMENT_TITLE_DURATION_SECONDS;
    }

    public bool IsExpired { get; private set; }

    public void Draw(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        if (IsExpired)
        {
            return;
        }

        var sourceMovementTitle = GameObjectHelper.GetLocalized(_combatMovementSid);
        var size = _font.MeasureString(sourceMovementTitle);

        var t = 1 - _usedCombatMovementCounterSeconds / COMBAT_MOVEMENT_TITLE_DURATION_SECONDS;

        var position = _isHeroSide
            ? new Vector2(contentRectangle.Left + ControlBase.CONTENT_MARGIN,
                contentRectangle.Top + ControlBase.CONTENT_MARGIN)
            : new Vector2(contentRectangle.Right - ControlBase.CONTENT_MARGIN - size.X,
                contentRectangle.Top + ControlBase.CONTENT_MARGIN);

        var color = Color.Lerp(Color.Transparent, MythlandersColors.MainSciFi, (float)Math.Cos(t * Math.PI * 0.5));

        spriteBatch.DrawString(_font, sourceMovementTitle, position, color);
    }

    public void Update(GameTime gameTime)
    {
        if (IsExpired)
        {
            return;
        }

        if (_usedCombatMovementCounterSeconds > 0)
        {
            _usedCombatMovementCounterSeconds -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            IsExpired = true;
        }
    }
}